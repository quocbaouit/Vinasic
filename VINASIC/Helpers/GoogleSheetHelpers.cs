using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using VINASIC.ViewModels;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace VINASIC.Helpers
{
  
    public class ClientSecret
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
        public ClientSecret()
        {
            type = "service_account";
            project_id = "yowsi-35162";
            private_key_id = "70a126515cdd595311a6c51fde8ac36743604bb1";
            private_key = "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCnNof4chzLzVLY\nZa7P7SkvPJqq2U31GHpelRxesOaW5mjBvsJbj7PrX2fxBrv8n2oNrOjA/L8jVLFT\nAlv1yfNPWN7xC7WbWMXXHggE/WReCURzpyy9c1jLUXWvi2w/MWpomJQIN0MH6yat\nEDqhX9JPGejzZksuS9wFVe2G7D17U2a3LLXFCc2JgppGbAcwQVs6o/SXWdsSfvBo\nN/Ch1ib+WE0LXtfp+e4TnDGNQgomzyWk0n23cS21yEzDUqkKYZUUVsUG0+UVtHxa\n/mkPrkCOTQ5Q3kN0y1bWhTWbzoQ5jZEqMb9TN/YCPIhi+GAjtiYeoamtNe52hu8E\ntU33rfJNAgMBAAECggEAJAPDQng1ipHVM6WnAsLGeZWYjI7UUyNsC94a56w7ZioH\nSx+OlhD8BAifBxNXvwAI5BwnX5Wwh2gQ71uVOfm+zYrLLejDO0viBMTDnmuDSBu/\n4hcSNFXZPKV1bxNdA7ldfVzLmX9pkRWNVX4m2qYcUow2l+Ii9DxDjeSljcdpMJv7\nSt6M7gILZ5PjOqoMVphUGXD1T1GaCyAS2yBJhS4vzpHE9tbmOZTi5cux8IJ+bX49\nXd44fBst6nimg96TwsWKFXpPRWe2NO4G6NccoC4fKo3BCWO7GI1HWoo5h3zlcDmj\nhIbL+a8AKQ+qPu/9U7ajWhwit29pPdx6mbLUJZzmMwKBgQDZPYKpEFiT21PNzWiR\nPjkoVjoqNUSrRMhJKzVZHZfZFjmfQmb7ehN1zftIkcGADyqjBD8ECbTbjCFJ1Ryq\nFq9mlrqCDRXuePxdHEHASSQKXnR/oRUuexWeVCozaPsZYhLR797VUX9QkV+4eimj\n40ZvSA7hndEpsFKjieYFBQKbNwKBgQDFDAQr6G0LVNbQok3rHG5iEYHxesIuKs3N\n6PALiKM7XfLaNRkadn/yZaoOM/qmnuO1QI/gwh4cQBzcKgydtX4rf3ZTUkO9/3BV\nT6hUT3sks1WDbjyeQF7OI8iO8uplqWKczXJ9LnRSRIOPEi5H0slE4CPv1M/N/dLc\nAMXymmHImwKBgQDPPH2DuGM3kZhtkJE7VUeDclaEY1KBq9kA6+Y1gCSWZDxN8DN7\nnOYpkHkxrlIQTueWoXtX58aPVit/WnzAyWlEDXAYllEgsGvWixkyTNPDzH2IL2uc\nbrCd3J5xcqlven6HpTNG/jrc6gGNY4SLoklT6ULA+iNJiLoc5nT2vuPQmwKBgQCt\nlstp/wVpcSEZhZKNKGvZ+0B1gsbkMxNBbg9iqcnbeAWE1Dg8Rt6qpYNtpb8P4HQL\n7keSIpCyJI0ILAcXPJddLJoKBzPkgs7saROKysZaa55O/eNrKpwakA2UWU3Dp7hX\nGOreevlArFkVxld9MKXdBnTCSMGU4CJNbGT2N6kjNwKBgGWM9nXMwuK/bjItnrAs\nbtx70fyXdIDIlvYAB5nfr2jC71PHA++gd+KGuirFR7PVLKUma5X4zIuF9zCM0KmN\n/+Bo53iLZ/wEdMKdxhhut/kqAwBLbLXWKsuL4+HuAgf9pyIxa+stYAFQ/ylrQsRl\n0nVR5c4LJJ3y3F2/2flIhz9H\n-----END PRIVATE KEY-----\n";
            client_email = "sheets@yowsi-35162.iam.gserviceaccount.com";
            client_id = "111782762291172203233";
            auth_uri = "https://accounts.google.com/o/oauth2/auth";
            token_uri = "https://oauth2.googleapis.com/token";
            auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs";
            client_x509_cert_url = "https://www.googleapis.com/robot/v1/metadata/x509/sheets%40yowsi-35162.iam.gserviceaccount.com";
        }

    }
    public interface IGoogleSheetHelpers
    {
        void ReadPpAndDecalEntries();
        IEnumerable<StampModel> ReadStamplEntries();
        IEnumerable<StandeeModel> ReadStandeeEntries();
        IEnumerable<DesignModel> ReadDesignEntries();
        IEnumerable<HiflexModel> ReadHiflexEntries();
        IEnumerable<Paper> ReadPaperEntries();
    }
    public class GoogleSheetHelpers: IGoogleSheetHelpers
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "SieuViet";
        static readonly string SpreadsheetId = "10MX-Gf_p3vLOfG2hkP9a0urmO5R0A5-Tu6LgX27QXBI";

        static SheetsService service;
        public  GoogleSheetHelpers()
        {
            GoogleCredential credential;
             credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new ClientSecret())).CreateScoped(Scopes);


            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            ReadPpAndDecalEntries();
        }
        public void ReadPpAndDecalEntries()
        {
            string sheet = "DaiLy";
            var range = $"{sheet}!C16:C18";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    //var ppAndDecal = new PpAndDecalModel();
                    //ppAndDecal.Name = values[i][0].ToString();
                    //ppAndDecal.Machining = int.Parse(values[i][2].ToString());
                    //ppAndDecal.Code = values[i][3].ToString();
                    //ppAndDecal.Lower5price = float.Parse(values[i][4].ToString()) * 1000;
                    //ppAndDecal.Between5And20price = float.Parse(values[i][5].ToString())*1000;
                    //ppAndDecal.Between20And50price = float.Parse(values[i][6].ToString()) * 1000;
                    //ppAndDecal.Between50And100price = float.Parse(values[i][7].ToString()) * 1000;
                    //ppAndDecal.Between100And200price = float.Parse(values[i][8].ToString()) * 1000;
                    //ppAndDecal.Between200And500price = float.Parse(values[i][9].ToString()) * 1000;
                    //ppAndDecal.Between500And1000price = float.Parse(values[i][10].ToString()) * 1000;
                    //ppAndDecal.Over1000price = float.Parse(values[i][11].ToString()) * 1000;
                    //ppAndDecal.Description = values[i][12].ToString();
                    //ppAndDecal.ImagePath = values[i][13].ToString();
                    //ppAndDecal.ImagePath2 = values[i][14].ToString();
                    //ppAndDecal.ImagePath3 = values[i][15].ToString();
                    //yield return ppAndDecal;
                }
            }
        }
        public IEnumerable<StampModel> ReadStamplEntries()
        {
            string sheet = "TemNhan";
            var range35 = $"{sheet}!A6:F18";
            var range36 = $"{sheet}!A21:F33";
            var range46 = $"{sheet}!H6:M18";
            var range58 = $"{sheet}!H21:M33";
            var range510 = $"{sheet}!A36:F48";
            List<StampModel> listStamp = new List<StampModel>();
            listStamp.AddRange(ReadStamplEntriesByRange(range35,"3cm 5cm",35,(float)0.03, (float)0.05));
            listStamp.AddRange(ReadStamplEntriesByRange(range36,"3cm 6cm",36, (float)0.03, (float)0.06));
            listStamp.AddRange(ReadStamplEntriesByRange(range46,"4cm 6cm",46, (float)0.04, (float)0.06));
            listStamp.AddRange(ReadStamplEntriesByRange(range58,"5cm 8cm",58, (float)0.05, (float)0.08));
            listStamp.AddRange(ReadStamplEntriesByRange(range510,"5cm 10cm",510, (float)0.05, (float)0.1));
            return listStamp;
        }
        private IEnumerable<StampModel> ReadStamplEntriesByRange(string range,string name,int code,float height,float width)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var stamp = new StampModel();
                    stamp.Name = name;
                    stamp.Code = code;
                    stamp.Height = height;
                    stamp.Width = width;
                    stamp.Square = float.Parse(values[i][2].ToString()); ;
                    stamp.Quantity = float.Parse(values[i][3].ToString());
                    stamp.Price = float.Parse(values[i][4].ToString());
                    stamp.Total = float.Parse(values[i][5].ToString());
                    stamp.SubTotal = float.Parse(values[i][5].ToString());
                    yield return stamp;
                }
            }
        }

        public IEnumerable<StandeeModel> ReadStandeeEntries()
        {
            string sheet = "Standee";
            var range = $"{sheet}!A4:H30";
            SpreadsheetsResource.ValuesResource.GetRequest request =service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var standee = new StandeeModel();
                    if (values[i].Count>2)
                    {
                        standee.Name = values[i][0].ToString();
                        standee.Code = values[i][1].ToString();
                        standee.Type = values[i].Count > 2 ? values[i][2].ToString():"0";
                        standee.Lower5price = values[i].Count > 3  ? float.Parse(values[i][3].ToString()) : 0;
                        standee.Between5And10price = values[i].Count > 4  ? float.Parse(values[i][4].ToString()) : 0;
                        standee.Between10And20price = values[i].Count > 5 ? float.Parse(values[i][5].ToString()) : 0;
                        standee.Between20And50price = values[i].Count > 6 ? float.Parse(values[i][6].ToString()) : 0;
                        standee.Over50price = values[i].Count > 7 ? float.Parse(values[i][7].ToString()) : 0;
                        yield return standee;
                    }                   
                }
            }
        }
        public IEnumerable<DesignModel> ReadDesignEntries()
        {
            string sheet = "Design";
            var range = $"{sheet}!A4:H12";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var design = new DesignModel();
                    if (values[i].Count > 2)
                    {
                        design.Name = values[i][0].ToString();
                        design.Code = values[i][1].ToString();
                        design.Lower2price = values[i].Count > 2 ? float.Parse(values[i][2].ToString()) : 0;
                        design.Between2And5price = values[i].Count > 3 ? float.Parse(values[i][3].ToString()) : 0;
                        design.Between5And10price = values[i].Count > 4 ? float.Parse(values[i][4].ToString()) : 0;
                        design.Between10And20price = values[i].Count >5 ? float.Parse(values[i][5].ToString()) : 0;
                        design.Between20And30price = values[i].Count > 6 ? float.Parse(values[i][6].ToString()) : 0;
                        design.Over30price = values[i].Count > 7 ? float.Parse(values[i][7].ToString()) : 0;
                        yield return design;
                    }
                }
            }
        }
        public IEnumerable<HiflexModel> ReadHiflexEntries()
        {
            string sheet = "Hiflex";
            var range = $"{sheet}!A2:J13";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var hiflex = new HiflexModel();
                    hiflex.Name = values[i][0].ToString();
                    hiflex.Machining = int.Parse(values[i][2].ToString());
                    hiflex.Code = values[i][3].ToString();
                    hiflex.Lower20price = float.Parse(values[i][4].ToString()) * 1000;
                    hiflex.Between20And100price = float.Parse(values[i][5].ToString()) * 1000;
                    hiflex.Between100And500price = float.Parse(values[i][6].ToString()) * 1000;
                    hiflex.Between500And1000price = float.Parse(values[i][7].ToString()) * 1000;
                    hiflex.Between1000And2000price = float.Parse(values[i][8].ToString()) * 1000;
                    hiflex.Over2000price = float.Parse(values[i][9].ToString()) * 1000;
                    yield return hiflex;
                }
            }
        }
        public IEnumerable<Paper> ReadPaperEntries()
        {
            string sheet = "Paper";
            var range = $"{sheet}!A5:D12";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var paper = new Paper();
                    paper.Name = values[i][0].ToString();
                    paper.Code = values[i][0].ToString().ToLower();
                    paper.Between1And100price = float.Parse(values[i][1].ToString());
                    paper.Between100And500price = float.Parse(values[i][2].ToString());
                    paper.Between500And1000price = float.Parse(values[i][3].ToString());
                    yield return paper;
                }
            }
        }
    }
}
