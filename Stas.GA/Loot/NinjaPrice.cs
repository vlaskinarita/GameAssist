using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
namespace Stas.GA;
//https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
//https://poe.ninja/swagger/index.html#/Data/Data_CurrencyOverview
public class PoeNinja : iSett, IDisposable {
    public Dictionary<string, List<NinjaPrice>> prices { get; set; } = new Dictionary<string, List<NinjaPrice>>();
    List<NinjaPrice> curr_price = new List<NinjaPrice>();
    public float exa_rate { get; set; }
    public float divine_rate { get; set; }
    public float alchemy_rate { get; set; }
    public DateTime upd_time { get; set; }
    [JsonIgnore]
    public bool b_ready = true;
    HttpClient client;
    List<(string, string)> priceQueue { get; set; }
    public PoeNinja() {
        //TestItem();
        client = new HttpClient();
        //Check(); //here for debug in start league
    }
    public async void Check() {
        if (upd_time.AddHours(3) > DateTime.Now || !b_ready)
            return;
        b_ready = false;
        prices.Clear();
        curr_price.Clear();
        var done = 0f;
        if (priceQueue == null) {
            priceQueue = JsonConvert.DeserializeObject<List<(string, string)>>(
                File.ReadAllText(ui.sett.ninja_price));
        }

        foreach (var q in priceQueue) {
            var uri = "https://poe.ninja/api/data/" + q.Item1 + "overview?league=" + ui.sett.curr_league + "&type=" + q.Item2;
            await GetFromUrl(uri, q.Item1, q.Item2);
            await Task.Delay(200);
            done += 1;
            ui.AddToLog("Ninja.Check [" + (done / priceQueue.Count).ToRoundStr(2) + "]");
        }
        upd_time = DateTime.Now;

        MakeDictionary();
        Save();
        b_ready = true;
    }
    void MakeDictionary() {
        foreach (var p in curr_price) {
            if (prices.ContainsKey(p.name)) {
                prices[p.name].Add(p);
            }
            else {
                prices[p.name] = new List<NinjaPrice>() { p };
            }
        }
    }
    async Task GetFromUrl(string uri, string task_type, string task_name) {
        string resp = null;
        try {
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            resp = await response.Content.ReadAsStringAsync();
        }
        catch (Exception) {
            ui.AddToLog("GetFromUrl=" + uri, MessType.Error);
            return;
        }


        if (task_type == "currency") {
            var surrencys = JsonConvert.DeserializeObject<CurrencyOverviewModel>(resp);
            foreach (var r in surrencys.lines) {
                var np = new NinjaPrice() { name = r.currencyTypeName, value = r.chaosEquivalent };
                curr_price.Add(np);
                if (r.detailsId == "exalted-orb") {
                    exa_rate = r.chaosEquivalent;
                }
                if (r.detailsId == "divine-orb") {
                    divine_rate = r.chaosEquivalent;
                }
                if (r.detailsId == "orb-of-alchemy") {
                    alchemy_rate = r.chaosEquivalent;
                }
            }
        }
        if (task_type == "item") {
            var items = JsonConvert.DeserializeObject<ItemOverviewModel>(resp);
            foreach (var r in items.lines) {
                var np = new NinjaPrice() { name = r.name, value = r.chaosValue };
                if (task_name == "Watchstone") np.count = r.count;
                if (task_name == "Map") { np.mapTier = r.mapTier; }
                if (task_name == "DivinationCard") { np.stackSize = r.stackSize; }
                if (task_name == "UniqueWeapon" || task_name == "UniqueArmour" || task_name == "UniqueAccessory") {
                    np.levelRequired = r.levelRequired;
                }
                if (task_name == "BaseType") {
                    np.levelRequired = r.levelRequired; np.variant = r.variant;
                }
                if (task_name == "SkillGem") {
                    np.gemLevel = r.gemLevel;
                    np.gemQuality = r.gemQuality; np.corrupted = r.corrupted;
                }
                curr_price.Add(np);
            }
        }
        //ui.AddToLog("Done " + task_name);
    }
 

    public void Dispose() {
        client.Dispose();
    }

    public class ItemOverviewModel {
        public class ItemOverviewLineModel {
            public int id;
            public string name;
            public string icon;
            public SparkLineInfo sparkline;
            public float chaosValue;
            public int mapTier;
            public int gemLevel;
            public int gemQuality;
            public float exaltedValue;
            public int count;
            public int stackSize;
            public int levelRequired;
            public bool corrupted;
            public string variant;
        }
        public ItemOverviewLineModel[] lines;
    }

    public class CurrencyOverviewModel {
        public CurrencyOverviewLineModel[] lines;
        public List<CurrencyDetailsModel> currencyDetails;

    }
    public class CurrencyDetailsModel {
        public int id;
        public string icon;
        public string name;
        public string tradeId;
    }


    public class SparkLineInfo {
        public float?[] data;
        public float number;
    }
    public class CurrencyDataPoint {
        public long id;
        public int league_id;
        public int pay_currency_id;
        public int get_currency_id;
        public int count;
        public float value;
        public int data_point_count;
        public bool includes_secondary;
        public int listing_count;
    }
    public class CurrencyOverviewLineModel {
        public string currencyTypeName;
        public CurrencyDataPoint receive;
        public CurrencyDataPoint pay;
        public SparkLineInfo paySparkLine;
        public SparkLineInfo receiveSparkLine;
        public float chaosEquivalent;
        public SparkLineInfo lowConfidencePaySparkLine;
        public SparkLineInfo lowConfidenceReceiveSparkLine;
        public string detailsId;

        public override string ToString() {
            return currencyTypeName + " " + chaosEquivalent.ToRoundStr(2);
        }
    }
}
public class NinjaPrice {
    public string icon { get; set; }
    public float value { get; set; }
    public string name { get; set; }
    public int count { get; set; }
    public int mapTier { get; set; }
    public int stackSize { get; set; }
    public int gemQuality { get; set; }
    public int gemLevel { get; set; }
    public int levelRequired { get; set; }
    public bool corrupted { get; set; }
    public string variant { get; set; }

    public override string ToString() {
        return name + " " + value.ToRoundStr(2);
    }
}