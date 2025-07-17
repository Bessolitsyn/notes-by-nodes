using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using notes_by_nodes.AppRules;
using notes_by_nodes.Entities;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes.UseCases;
using VDS.RDF.Configuration;

namespace TestProject.UnitTests
{
    //Test interactors(use cases) and storage
    //TO DO переделать в модульные тесты, варианты использования нужно тестировать в тестах компонентов
    //или приемочных тестах (тестах построенных по требованиям к ПО)

    public class CoreInteractorTest
    {
        static LocalUser? _user;
        [Fact]
        public static void TestStartAppWhenNoUser()
        {

            foreach (string item in Directory.GetFiles(TestAppCore.profileFolder))
            {
                if (item.Contains(".luser"))
                    System.IO.File.Delete(item);

            }
            
            using (var testapp = new TestAppCore())
            {
                Assert.True(testapp != null);
            }
        }
        
        [Fact]
        public static void TestStartAppWhenThereIsUser()
        {
            TestStartAppWhenNoUser(); // здесь создается пользователь

            using (var testapp = new TestAppCore())
            {
                Assert.True(Directory.GetFiles(TestAppCore.profileFolder).Where(f => f.Contains(".luser")).Count() == 1 
                    && testapp.currentUser.Name == "Anton");
            }
        }
        //Требует что бы выполнились предыдущие
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
                var box = testapp.NewBoxIfUserHaveNoBox();
                Assert.True(testapp?.currentUser?.HasChildNodes.Contains(box));
                
            }
        }

        [Fact]
        //Требует что бы выполнились предыдущие
        public static void TestAddNewNotesToBox()
        {
            TestStartBoxaCreating();
            using (var testapp = new TestAppCore())
            {

                var box = testapp?.currentUser?.HasChildNodes.First() as LocalBox;
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
                testapp.storageFactory.GetNoteStorage(box).LoadChildNodesAsync(box).Wait();
                foreach (var note in box.HasChildNodes)
                {
                    testapp.storageFactory.GetNoteStorage(box).LoadChildNodesAsync(note).Wait();

                }
                
                Assert.True(
                    box.HasChildNodes.Count() == 2 &&
                    box.HasChildNodes.Single(n => n.Name == "The Title2").HasChildNodes.Count() == 1
                    );
            }
        }

    }

    class TestAppCore : IDisposable
    {
        public static string profileFolder = Directory.GetCurrentDirectory().ToString() + $"\\..\\..\\..\\FilesStorage";
        public LocalUser currentUser;
        public NodeFileStorageProvider storageFactory;
        //Lazy
        CoreInteractor interactor;
        UserInteractor userInteractor;
        public TestAppCore()
        {
            INodeBuilder nodeBuilder = new NodeBuilder();
            storageFactory = new NodeFileStorageProvider(nodeBuilder, profileFolder);
            userInteractor = new UserInteractor(storageFactory);
            currentUser = SelectUser().Result; 
            interactor = new CoreInteractor(storageFactory, currentUser);

        }


        public async Task<LocalUser> SelectUser()
        {
            try
            {
                currentUser = await userInteractor.GetUser("Anton");
                //userInteractor.Chu
            }
            catch (Exception)
            {
                currentUser = await userInteractor.MakeUser("Anton", "Anton@mail");
            }
            return currentUser;
          
        }

        public LocalBox NewBoxIfUserHaveNoBox()
        {
            var boxes = interactor.GetBoxes();
            var bxStorage = storageFactory.GetBoxStorage();
            if (boxes.Count() == 0)
            {
                return MakeBox();
            }
            else
                throw new Exception("User have more than one box");

            LocalBox MakeBox()
            {
                string boxFolder = Directory.GetCurrentDirectory().ToString() + $"\\..\\..\\..\\FilesStorage\\Box";
                Directory.CreateDirectory(boxFolder);
                LocalBox box = new(currentUser, boxFolder, "FirstTestBox");
                //box.SetNoteStorage(storageFactory.GetNoteStorage(box));
                bxStorage.SaveBoxAsync(box).Wait();
                currentUser.AddIntoChildNodes(box);
                storageFactory.GetUserStorage().SaveUserAsync(currentUser).Wait();
                boxes = interactor.GetBoxes();
                return boxes.First();
            }

        }

        public LocalNote NewNoteToBox(LocalBox box, string name, string text, string desc)
        {
            var note = new LocalNote(box, name, desc);
            note.Text = text;
            var noteStorage = storageFactory.GetNoteStorage(box);
            noteStorage.SaveNoteAsync(note).Wait();
            //box.AddIntoChildNodes(note);
            storageFactory.GetBoxStorage().SaveBoxAsync(box).Wait();
            return note;
        }
        public LocalNote NewNoteToNote(LocalBox box, LocalNote pnote, string name, string text, string desc)
        {
            var note = new LocalNote(pnote, name, desc);
            note.Text = text;
            //var noteStorage = pnote.NoteStorage;
            storageFactory.GetNoteStorage(box).SaveNoteAsync(note).Wait();
            storageFactory.GetNoteStorage(box).SaveNoteAsync(pnote).Wait();
            return note;                               
        }
        public void Dispose()
        {
            storageFactory.Dispose();
        }
    }
}
