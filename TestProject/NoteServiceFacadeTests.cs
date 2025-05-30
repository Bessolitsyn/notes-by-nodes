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
        public static NoteServiceFacade TestFacade { get => _testFacade ?? setTestFasde(); }
        static NoteServiceFacade? _testFacade;
        public static NoteServiceFacade setTestFasde()
        {

            _testFacade = new NoteServiceFacade(new TestStorageFactory());
            return _testFacade;

        }

        [Fact]
        public static async Task StartAppWhenNoUserAsync()
        {
            try
            {


                IUserDto newuser = testApp.TestFacade.NewUser(new UserDto(0, "ayur", ""));
                testApp.TestFacade.SelectUser(newuser.Uid);
                var boxfolder = Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\Box\\";

                var box = testApp.TestFacade.NewBox(new NodeDto(0, boxfolder, "", ""));
                var childs = testApp.TestFacade.GetChildNodesOfTheBox(box.Uid);
                var note1 = await testApp.TestFacade.NewNote(box.Uid, box.Uid);
                //var childs2 = testApp.TestFacade.GetChildNodesOfTheBox(box.Uid);
                var note2 = await testApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note3 = await testApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note31 = await testApp.TestFacade.NewNote(box.Uid, note3.Uid);
                var note32 = await testApp.TestFacade.NewNote(box.Uid, note3.Uid);
                await testApp.TestFacade.Remove(box.Uid, note1.Uid);
                await testApp.TestFacade.Remove(box.Uid, note2.Uid);
                await testApp.TestFacade.Remove(box.Uid, note3.Uid);
                await testApp.TestFacade.Remove(box.Uid);

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
    class testApp
    {
        public static NoteServiceFacade TestFacade { get => _testFacade ?? setTestFasde(); }
        static NoteServiceFacade? _testFacade;
        public static NoteServiceFacade setTestFasde() {

            _testFacade = new NoteServiceFacade( new TestStorageFactory());
            return _testFacade;

        }


    }
    internal class TestStorageFactory : INodeStorageProvider
    {
        private readonly INodeStorageProvider _storageFactory;
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
