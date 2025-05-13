using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes.UseCases;
using VDS.RDF.Configuration;

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
                    System.IO.File.Delete(item);

            }

            using var testapp = new TestAppCore();
            Assert.True(testapp != null);
        }

        [Fact]
        public static void TestStartAppWhenThereIsUser()
        {
            using (var testapp = new TestAppCore())
            {
                foreach (string item in Directory.GetFiles(TestAppCore.profileFolder))
                {
                    if (item.Contains(testapp.currentUser.Uid.ToString()))
                        Assert.True(true);
                    return;
                }
            }
            Assert.True(false);
        }
        [Fact]
        public static void TestStartBoxaCreating()
        {
            foreach (string item in Directory.GetFiles(TestAppCore.profileFolder))
            {
                if (item.Contains(".lbox"))
                    System.IO.File.Delete(item);
            }

            using (var testapp = new TestAppCore())
            {
                var box = testapp.NewBox();
                Assert.True(testapp.currentUser.HasChildNodes.Contains(box));
                
            }
        }

        [Fact]
        //Требует что бы выполнились предыдущие
        public static void TestAddNewNotesToBox()
        {
            using (var testapp = new TestAppCore())
            {
                
                var box = testapp.currentUser.HasChildNodes.First() as LocalBox;
                foreach (string item in Directory.GetFiles(box.Name))
                {
                    if (item.Contains(".lnote"))
                        System.IO.File.Delete(item);
                }
                testapp.NewNoteToBox(box, "The Title1", "Any text of title", "Description");
                testapp.NewNoteToBox(box, "The Title2", "Any text of title", "Description");
                LocalNote note2 = box.HasChildNodes.FirstOrDefault(n => n.Name == "The Title2") as LocalNote;
                var note3 = testapp.NewNoteToNote(box, note2, "The Title3", "Any text of title", "Description");

            }
            using (var testapp = new TestAppCore())
            {

                var box = testapp.currentUser.HasChildNodes.First() as LocalBox;
                testapp.storageFactory.GetNoteStorage(box).LoadChildNodes(box);
                foreach (var note in box.HasChildNodes)
                {
                    testapp.storageFactory.GetNoteStorage(box).LoadChildNodes(note);

                }
                //box.LoadChildNodes();
                
                Assert.True(
                    box.HasChildNodes.Count() == 2 &&
                    box.HasChildNodes.FirstOrDefault(n => n.Name == "The Title2").HasChildNodes.Count() == 1
                    //note2.HasChildNodes.Contains(note3) &&
                    //note3.HasOwner == testapp.currentUser
                    );
            }
        }

    }

    class TestAppCore : IDisposable
    {
        public static string profileFolder = System.IO.Directory.GetCurrentDirectory().ToString() + $"\\..\\..\\..\\FilesStorage";
        public LocalUser? currentUser;
        public NodeFileStorageFactory storageFactory;
        //Lazy
        CoreInteractor interactor;
        UserInteractor userInteractor;
        public TestAppCore()
        {
            INodeBuilder nodeBuilder = new NodeBuilder();
            storageFactory = new NodeFileStorageFactory(nodeBuilder, profileFolder);
            userInteractor = new UserInteractor(storageFactory);
            currentUser = SelectUser();            
            interactor = new CoreInteractor(storageFactory, currentUser);



            //UseCase user selecting  


        }
        LocalUser SelectUser()
        {
            var users = userInteractor.GetUsers();
            if (users.Length == 0)
            {
                var newUser = userInteractor.MakeUser("Anton", "Anton@mail");
                newUser = userInteractor.GetUsers().SingleOrDefault(u => u.Uid == newUser.Uid) ?? throw new NullRefernceUseCaseException("user creating error");
                return newUser;
            }
            else { 
                var selectedUser = users.First();
                storageFactory.GetBoxStorage().LoadChildNodes(selectedUser);
                return selectedUser;           
            
            }
        }

        public LocalBox NewBox()
        {
            var boxes = interactor.GetBoxes();
            var bxStorage = storageFactory.GetBoxStorage();
            if (boxes.Count() == 0)
            {
                return MakeBox();
            }
            throw new Exception("Unknown error");

            LocalBox MakeBox()
            {
                string boxFolder = System.IO.Directory.GetCurrentDirectory().ToString() + $"\\..\\..\\..\\FilesStorage\\Box";
                LocalBox box = new(currentUser, boxFolder, "FirstTestBox");
                //box.SetNoteStorage(storageFactory.GetNoteStorage(box));
                bxStorage.SaveBox(box);
                currentUser.AddIntoChildNodes(box);
                storageFactory.GetUserStorage().SaveUser(currentUser);
                boxes = interactor.GetBoxes();
                return boxes.First();

            }

        }

        public LocalNote NewNoteToBox(LocalBox box, string name, string text, string desc)
        {
            var note = new LocalNote(box, name, desc);
            note.Text = text;
            var noteStorage = storageFactory.GetNoteStorage(box);
            noteStorage.SaveNote(note);
            //box.AddIntoChildNodes(note);
            storageFactory.GetBoxStorage().SaveBox(box);
            return note;
        }
        public LocalNote NewNoteToNote(LocalBox box, LocalNote pnote, string name, string text, string desc)
        {
            var note = new LocalNote(pnote, name, desc);
            note.Text = text;
            //var noteStorage = pnote.NoteStorage;
            storageFactory.GetNoteStorage(box).SaveNote(note);
            storageFactory.GetNoteStorage(box).SaveNote(pnote);
            return note;
        }
        public void Dispose()
        {
            storageFactory.Dispose();
        }
    }
}
