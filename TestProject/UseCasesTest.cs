using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes.UseCases;
using notes_by_nodes.UseCases.AppRules;

namespace TestProject
{
    public class UseCasesTest
    {
        [Fact]
        public static void TestStartAppWhenNoUser()
        {
            foreach (string item in Directory.GetFiles(TestAppCore.profileFolder))
            {
                if (item.Contains(".luser"))
                    File.Delete(item);
            }

            var testapp = new TestAppCore();
            Assert.True(testapp != null);
        }

        [Fact]
        public static void TestStartAppWhenThereIsUser()
        {
            var testapp = new TestAppCore();
            foreach (string item in Directory.GetFiles(TestAppCore.profileFolder))
            {
                if (item.Contains(TestAppCore.currentUser.Uid.ToString()))
                    Assert.True(true);
                return;
            }
            Assert.True(false);
        }
    }

    class TestAppCore : IDisposable
    {
        public static string profileFolder = System.IO.Directory.GetCurrentDirectory().ToString()+$"\\..\\..\\..\\FilesStorage";
        public static LocalUser? currentUser;
        //Lazy
        InsideInteractor interactor;
        List<string> localBoxes = [];
        public TestAppCore()
        {
            NodeFileStorageFactory storageFactory = new NodeFileStorageFactory(profileFolder);

            currentUser = SelectUser();
            interactor = new InsideInteractor(storageFactory, currentUser.Uid);
            var boxes = interactor.GetBoxes();


            //UseCase user selecting  
            LocalUser SelectUser()
            {
                var userStorage = storageFactory.GetUserStorage();
                var users = userStorage.GetUsers();
                if (users.Length == 0)
                {
                    return MakeUserProfile();

                    //throw new Exception("No users. Please make user profile to continue");
                }
                return users.First();


                LocalUser MakeUserProfile()
                {
                    LocalUser user = new("Anton", "Anton@mail", storageFactory.GetBoxStorage());
                    storageFactory.GetUserStorage().SaveUser(user);
                    users = userStorage.GetUsers();
                    return user;
                }
            }


        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
