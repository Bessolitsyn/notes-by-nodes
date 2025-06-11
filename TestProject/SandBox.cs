using OwlToT4templatesTool;
namespace TestProject
{
    public class SandBox
    {
        [Fact]
        public void RunSandBox()
        {
            var myrecord = new Dataset2("Name", "Number", "Adress");
            var myrecord_ = Newtonsoft.Json.JsonConvert.SerializeObject(myrecord);
            var myrecord_s = Newtonsoft.Json.JsonConvert.DeserializeObject<Dataset2>(myrecord_);
            Assert.Equal(myrecord,myrecord_s);
        }


        public partial record Dataset1(string Name, string Number);
        public partial record Dataset2(string Name, string Number, string Adress) : Dataset1(Name, Number);
    }
}