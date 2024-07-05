using analyze.core.api.Contexts;
using analyze.core.Models.Daily;
using analyze.core.Outputs;


namespace analyze.core.api.Data
{
    public  class DbInitializer : IOutput
    {
        public  void Initialize(AnalyzeContext context)
        {
            // Look for any students.
            if (context.DailyDetails.Any())
            {
                return;   // DB has been seeded
            }

            Analyzer analyzer = new Analyzer();
            analyzer.Output = new DbInitializer();
            analyzer.SetRootDirectorie("D:\\我的坚果云\\数据采集");
            analyzer.StartCollect(CollectTypes.Daily);
            DailyDetail[] dailyDetails = analyzer.GetDaily(DateTime.Now);

            context.DailyDetails.AddRangeAsync(dailyDetails);
            context.SaveChanges();

        }

        public void AddRow(params object[] objs)
        {
            //throw new NotImplementedException();
        }

        public void Append(string str)
        {
            //throw new NotImplementedException();
        }

        public void Show(params string[] headers)
        {
            //throw new NotImplementedException();
        }

        public string Write(string str, bool online = true)
        {
            //throw new NotImplementedException();
            return null;
        }

        public void WriteLine(string str = "")
        {
            //throw new NotImplementedException();
        }
    }
}
