using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Stas.GA;
using System.Text.Json.Serialization;


public partial class Settings : iSett {
   
    public Settings() {
    }
}
public partial class ui {
    /// <summary>
    /// need after gamestate was changed to select hero screen 
    /// </summary>
    public static void ResetWorker() {
        b_worker_err = false;
        worker = null;
        curr_player = null;
    }
    static Player curr_player;
    static void CheckWorker() {
        if (me.Address == default || b_worker_err! || !me.GetComp<Player>(out var _cp) || string.IsNullOrEmpty(_cp.Name)) {
            //Address== defaul possible if relogin fast - debug here mb
            ui.AddToLog(tName + ".CheckWorker err get curr player", MessType.Error);
            return;
        }
        if (worker == null || curr_player == null || _cp.Name != curr_player.Name) {
            worker = GetWorkerByName(_cp.Name);
            if (!b_worker_err)
                curr_player = _cp;
        }
    }
    static bool b_worker_err = false;
    static aWorker GetWorkerByName(string _name) {
        if (!b_worker_err) {
            try {
                var asm = Assembly.GetExecutingAssembly();//.GetTypes();
                var anme = "Stas.GA." + _name;
                var handle = Activator.CreateInstance(asm.FullName, anme);
                var w = handle.Unwrap();
                Type t = w.GetType();
                MethodInfo method = w.GetType().GetMethod("Load");
                method = method.MakeGenericMethod(t);
                Type[] arg = { null }; //число аргументов должно соотвествовать числу аргуметов в методе
                aWorker res = (aWorker)method.Invoke(w, null);
                return res;
            }
            catch (Exception ex) {
                b_worker_err = true;
                var pattern = "Could not load type '(.*?)' from assembly";
                var re = new Regex(pattern);
                var err = "GetWorkerByName err...";
                if (re.IsMatch(ex.Message))
                    err += (string)re.Matches(ex.Message)[0].Groups[1].Value;
                else
                    err += "=>" + ex.Message;
                ui.AddToLog(err, MessType.Critical);
                return null;
            }
        }
        return null;
    }
}
