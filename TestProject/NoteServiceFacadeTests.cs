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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Sdk;

namespace TestProject
{
    public class NoteServiceFacadeTests
    {
        [Fact]
        public static async Task StartAppWhenNoUserAsync()
        {
            try
            {                
                IUserDto newuser = await TestApp.TestFacade.NewUser(new UserDto(0, "TestUSer", ""));
                TestApp.TestFacade.SelectUser(newuser.Name);
                var boxfolder = Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\Box\\";

                var box = await TestApp.TestFacade.NewBox(new NodeDto(0, boxfolder, "", ""));
                var childs = await TestApp.TestFacade.GetChildNodesOfTheBox(box.Uid);
                var note1 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note2 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note3 = await TestApp.TestFacade.NewNote(box.Uid, box.Uid);
                var note31 = await TestApp.TestFacade.NewNote(box.Uid, note3.Uid);
                var note312 = await TestApp.TestFacade.NewNote(box.Uid, note31.Uid);
                await setName(note1, "note1");
                await setName(note2, "note2");
                await setName(note3, "note3");
                await setName(note31, "note31");
                await setName(note312, "note312");
                await TestApp.TestFacade.Remove(box.Uid, note1.Uid);
                await TestApp.TestFacade.Remove(box.Uid, note2.Uid);
                await TestApp.TestFacade.Remove(box.Uid, note3.Uid);
                await TestApp.TestFacade.Remove(box.Uid);

                async Task setName(INodeDto note, string name)
                {
                    note.Name = name; 
                    await TestApp.TestFacade.ModifyNote(box.Uid, note);                
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Assert.True(Directory.GetFiles(Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\Box\\").Length==0);
                foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\..\\..\\..\\FilesStorage2\\"))
                {
                    File.Delete(item);
                }
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
