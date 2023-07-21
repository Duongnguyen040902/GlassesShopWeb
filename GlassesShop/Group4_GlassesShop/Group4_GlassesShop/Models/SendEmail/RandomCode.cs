namespace DemoWebFirstMVCCore
{
    public class RandomCode
    {
        public string GetRandomCode()
        {
            Random r = new Random();
            int num = r.Next(999999);
            return string.Format("{0:D8}", num);
        }
    }
}
