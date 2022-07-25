﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using VINASIC.ViewModels;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;
using VINASIC.Data.Repositories;
using VINASIC.Data;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.Object;

namespace VINASIC.Helpers
{

    public class Entry
    {
        public string KeyName { get; set; }
        public string Range { get; set; }
    }
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
        void ReadEntries();
    }
    //public class GoogleSheetHelpers: IGoogleSheetHelpers
    //{
    //    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    //    static readonly string ApplicationName = "SieuViet";
    //    static readonly string SpreadsheetId = "10MX-Gf_p3vLOfG2hkP9a0urmO5R0A5-Tu6LgX27QXBI";
    //    private readonly IT_ContentRepository _repContent;
    //    private readonly IUnitOfWork<VINASICEntities> _unitOfWork;

    //    static SheetsService service;
    //    public GoogleSheetHelpers(IUnitOfWork<VINASICEntities> unitOfWork, IT_ContentRepository repContent)
    //    {
    //        GoogleCredential credential;
    //        credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new ClientSecret())).CreateScoped(Scopes);


    //        // Create Google Sheets API service.
    //        service = new SheetsService(new BaseClientService.Initializer()
    //        {
    //            HttpClientInitializer = credential,
    //            ApplicationName = ApplicationName,
    //        });
    //        ReadEntries();
    //    }
    //    public void ReadEntries()
    //    {
    //        string sheet = "DaiLy";

    //        List<Entry> entries = new List<Entry>();
    //        var ingiay = $"{sheet}!B16:C18";
    //        entries.Add(new Entry() { Range = ingiay, KeyName = "ingiay" });
    //        var indecalgiay = $"{sheet}!B23:C25";
    //        entries.Add(new Entry() { Range = indecalgiay, KeyName = "indecalgiay" });
    //        var indecalxi = $"{sheet}!B30:C32";
    //        entries.Add(new Entry() { Range = indecalxi, KeyName = "indecalxi" });
    //        var namecard1 = $"{sheet}!B37:C39";
    //        entries.Add(new Entry() { Range = namecard1, KeyName = "namecard1" });
    //        var namecard2 = $"{sheet}!B42:C43";
    //        entries.Add(new Entry() { Range = namecard2, KeyName = "namecard2" });

    //        var c80 = $"{sheet}!E16:F16";
    //        entries.Add(new Entry() { Range = c80, KeyName = "c80" });
    //        var c100 = $"{sheet}!E17:F17";
    //        entries.Add(new Entry() { Range = c100, KeyName = "c100" });
    //        var c120 = $"{sheet}!E18:F18";
    //        entries.Add(new Entry() { Range = c120, KeyName = "c120" });
    //        var c150 = $"{sheet}!E19:F19";
    //        entries.Add(new Entry() { Range = c150, KeyName = "c150" });
    //        var c200 = $"{sheet}!E20:F20";
    //        entries.Add(new Entry() { Range = c200, KeyName = "c200" });

    //        var c250 = $"{sheet}!E21:F21";
    //        entries.Add(new Entry() { Range = c250, KeyName = "c250" });
    //        var c300 = $"{sheet}!E22:F22";
    //        entries.Add(new Entry() { Range = c300, KeyName = "c300" });
    //        var b300 = $"{sheet}!E23:F23";
    //        entries.Add(new Entry() { Range = b300, KeyName = "b300" });
    //        var i300 = $"{sheet}!E24:F24";
    //        entries.Add(new Entry() { Range = i300, KeyName = "i300" });

    //        var decalgiay = $"{sheet}!E27:F27";
    //        entries.Add(new Entry() { Range = decalgiay, KeyName = "decalgiay" });
    //        var decalkraff = $"{sheet}!E28:F28";
    //        entries.Add(new Entry() { Range = decalkraff, KeyName = "decalkraff" });
    //        var decalnhua = $"{sheet}!E29:F29";
    //        entries.Add(new Entry() { Range = decalnhua, KeyName = "decalnhua" });
    //        var decalxi = $"{sheet}!E30:F30";
    //        entries.Add(new Entry() { Range = decalxi, KeyName = "decalxi" });
    //        var decal7mau = $"{sheet}!E31:F31";
    //        entries.Add(new Entry() { Range = decal7mau, KeyName = "decal7mau" });
    //        var decalbe = $"{sheet}!E32:F32";
    //        entries.Add(new Entry() { Range = decalbe, KeyName = "decalbe" });

    //        var baothugap1222 = $"{sheet}!E33:F33";
    //        entries.Add(new Entry() { Range = baothugap1222, KeyName = "baothugap1222" });
    //        var baothugap1623 = $"{sheet}!E34:F34";
    //        entries.Add(new Entry() { Range = baothugap1623, KeyName = "baothugap1623" });
    //        var baothugap2535 = $"{sheet}!E35:F35";
    //        entries.Add(new Entry() { Range = baothugap2535, KeyName = "baothugap2535" });
    //        var giaymythuat = $"{sheet}!E36:F36";
    //        entries.Add(new Entry() { Range = giaymythuat, KeyName = "giaymythuat" });
    //        var inhopmica = $"{sheet}!E37:F37";
    //        entries.Add(new Entry() { Range = inhopmica, KeyName = "inhopmica" });

    //        var canmang = $"{sheet}!H16:I22";
    //        entries.Add(new Entry() { Range = canmang, KeyName = "canmang" });
    //        var dongkim = $"{sheet}!H24:I30";
    //        entries.Add(new Entry() { Range = dongkim, KeyName = "dongkim" });
    //        var dongloxo = $"{sheet}!H32:I37";
    //        entries.Add(new Entry() { Range = dongloxo, KeyName = "dongloxo" });
    //        var dongkeogay = $"{sheet}!H39:I44";
    //        entries.Add(new Entry() { Range = dongkeogay, KeyName = "dongkeogay" });
    //        var canduonggap = $"{sheet}!H46:I51";
    //        entries.Add(new Entry() { Range = canduonggap, KeyName = "canduonggap" });
    //        var bedecal = $"{sheet}!H53:I57";
    //        entries.Add(new Entry() { Range = bedecal, KeyName = "bedecal" });
    //        var epplastic = $"{sheet}!H60:I64";
    //        entries.Add(new Entry() { Range = epplastic, KeyName = "epplastic" });


    //        var folder1 = $"{sheet}!B88:C94";
    //        entries.Add(new Entry() { Range = folder1, KeyName = "folder1" });
    //        var folder2 = $"{sheet}!B88:D94";
    //        entries.Add(new Entry() { Range = folder2, KeyName = "folder2" });
    //        var tieude = $"{sheet}!B99:C103";
    //        entries.Add(new Entry() { Range = tieude, KeyName = "tieude" });
    //        var baothuin1222 = $"{sheet}!B109:C115";
    //        entries.Add(new Entry() { Range = baothuin1222, KeyName = "baothuin1222" });
    //        var baothuin1623 = $"{sheet}!B109:D115";
    //        entries.Add(new Entry() { Range = baothuin1623, KeyName = "baothuin1623" });
    //        var baothuin2535 = $"{sheet}!B109:E115";
    //        entries.Add(new Entry() { Range = baothuin2535, KeyName = "baothuin2535" });

    //        var bieumau11 = $"{sheet}!B123:C128";
    //        entries.Add(new Entry() { Range = bieumau11, KeyName = "bieumau11" });
    //        var bieumau12 = $"{sheet}!B123:D128";
    //        entries.Add(new Entry() { Range = bieumau12, KeyName = "bieumau12" });
    //        var bieumau13 = $"{sheet}!B123:E128";
    //        entries.Add(new Entry() { Range = bieumau13, KeyName = "bieumau13" });
    //        var bieumau21 = $"{sheet}!B133:C138";
    //        entries.Add(new Entry() { Range = bieumau21, KeyName = "bieumau21" });
    //        var bieumau22 = $"{sheet}!B133:D138";
    //        entries.Add(new Entry() { Range = bieumau22, KeyName = "bieumau22" });
    //        var bieumau23 = $"{sheet}!B133:E138";
    //        entries.Add(new Entry() { Range = bieumau23, KeyName = "bieumau23" });
    //        ReadEntriesByRange(entries);
    //    }
    //    private void ReadEntriesByRange(List<Entry> entries)
    //    {
    //        List<ListProductPrice> listProductPrices = new List<ListProductPrice>();


    //        foreach (var entry in entries)
    //        {
    //            ListProductPrice productPrices = new ListProductPrice();
    //            productPrices.Code = entry.KeyName;
    //            try
    //            {
    //                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, entry.Range);
    //                var response = request.Execute();
    //                IList<IList<object>> values = response.Values;
    //                if (values != null && values.Count > 0)
    //                {
    //                    for (int i = 0; i < values.Count; i++)
    //                    {
    //                        ProductPrice product = new ProductPrice();
    //                        product.Id = new System.Guid();
    //                        product.Index = i + 1;
    //                        product.isFixed = false;
    //                        if (productPrices.Code == "canmang" && (i == 2 || i == 3))
    //                        {
    //                            product.isFixed = true;
    //                        }
    //                        if (productPrices.Code == "dongkeogay" && (i == 0 || i == 1))
    //                        {
    //                            product.isFixed = true;
    //                        }
    //                        if (productPrices.Code == "canduonggap" && (i == 1 || i == 2 || i == 3 || i == 4))
    //                        {
    //                            product.isFixed = true;
    //                        }

    //                        var listminMax = values[i][0]?.ToString().Split(new[] { "--" }, StringSplitOptions.None);

    //                        product.Min = Regex.Match(listminMax[0], @"\d+").Value;
    //                        product.Max = listminMax.Length > 1 ? Regex.Match(listminMax[1], @"\d+").Value : "0";
    //                        product.Price = Regex.Match(values[i][1]?.ToString(), @"\d+").Value ?? "0";
    //                        if (productPrices.Code == "folder1")
    //                        {
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "folder2")
    //                        {
    //                            product.Price = Regex.Match(values[i][2]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "tieude")
    //                        {
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "baothuin1623")
    //                        {
    //                            product.Price = Regex.Match(values[i][2]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "baothuin2535")
    //                        {
    //                            product.Price = Regex.Match(values[i][3]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "bieumau12")
    //                        {
    //                            product.Price = Regex.Match(values[i][2]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "bieumau13")
    //                        {
    //                            product.Price = Regex.Match(values[i][3]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "bieumau22")
    //                        {
    //                            product.Price = Regex.Match(values[i][2]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        if (productPrices.Code == "bieumau23")
    //                        {
    //                            product.Price = Regex.Match(values[i][3]?.ToString(), @"\d+").Value ?? "0";
    //                            product.Min = "0";
    //                            product.Max = Regex.Match(listminMax[0], @"\d+").Value;
    //                        }
    //                        productPrices.Products.Add(product);
    //                    }
    //                    listProductPrices.Add(productPrices);
    //                }
    //            }
    //            catch (System.Exception ex)
    //            {
    //                continue;
    //            }
    //        }
    //        var a = listProductPrices;

    //        var content = new T_Content();
    //        content.Name = "productPrice";
    //        content.Content = JsonConvert.SerializeObject(listProductPrices);
    //        content.Type = 2;
    //        content.CreatedDate = DateTime.Now.AddHours(14);
    //        _repContent.Add(content);
    //        _unitOfWork.Commit();
    //    }
    //}
}