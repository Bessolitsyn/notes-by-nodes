using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using notes_by_nodes.AppRules;
using notes_by_nodes.Dto;
using notes_by_nodes.Service;
using notes_by_nodes.Storage;
using notes_by_nodes.StorageAdapters;
using notes_by_nodes.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestProject
{
    public class NoteServiceFacadeTests
    {

        [Fact]
        public static async Task StartAppWhenNoUserAsync()
        {
            try
            {


                IUserDto newuser = TestApp.TestFacade.NewUser(new UserDto(0, "TestUSer", ""));
                TestApp.TestFacade.SelectUser(newuser.Uid);
                var boxfolder = Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\Box\\";

                var box = TestApp.TestFacade.NewBox(new NodeDto(0, boxfolder, "", ""));
                var childs = TestApp.TestFacade.GetChildNodesOfTheBox(box.Uid);
                var note1 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                //var childs2 = TestApp.TestFacade.GetChildNodesOfTheBox(box.Uid);
                var note2 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note3 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note31 = await TestApp.TestFacade.NewNote(box.Uid, note3.Uid);
                var note32 = await TestApp.TestFacade.NewNote(box.Uid, note3.Uid);
                await TestApp.TestFacade.Remove(box.Uid, note1.Uid);
                await TestApp.TestFacade.Remove(box.Uid, note2.Uid);
                await TestApp.TestFacade.Remove(box.Uid, note3.Uid);
                await TestApp.TestFacade.Remove(box.Uid);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Assert.True(true);
            }
        }

    }
    class TestApp
    {
        public static NoteServiceFacade TestFacade { get => _testFacade ?? SetTestFasade(); }
        static NoteServiceFacade? _testFacade;
        public static NoteServiceFacade SetTestFasade() {

            _testFacade = new NoteServiceFacade( new TestStorageFactory());
            return _testFacade;

        }


    }
    internal class TestStorageFactory : INodeStorageProvider
    {
        private readonly NodeFileStorageProvider _storageFactory;
        public TestStorageFactory()
        {
            INodeBuilder nodeBuilder = new NodeBuilder();
            var current = Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\";
            _storageFactory = new NodeFileStorageProvider(nodeBuilder, current);

        }

        public IBoxStorage GetBoxStorage()
        {
            return _storageFactory.GetBoxStorage();
        }

        public INoteStorage GetNoteStorage(LocalBox box)
        {
            return _storageFactory.GetNoteStorage(box);
        }

        public IUserStorage GetUserStorage()
        {
            return _storageFactory.GetUserStorage();
        }
    }

    

}
