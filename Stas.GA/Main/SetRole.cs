using System.Diagnostics;
using System.Net;

namespace Stas.GA;
public partial class ui {
    public static void SetRole() {
        switch (sett.role) {
            case Role.Master:
                tasker?.Stop("SetRole");
                udp_bot?.Dispose();
                //TODO temporary for debug old one
                tasker = new Master();
                var mname = Environment.MachineName;
                switch (mname) {
                    case "VOVA": //a4:50:56:2A:27:DB
                        udp_master = new UdpListener(IPAddress.Parse("192.168.1.100"));
                        break;
                    default: {
                            udp_master = new UdpListener(IPAddress.Parse(ui.sett.master_IP));
                            ui.AddToLog("SetRole err: You must specify the IP address through which the bots will connect to you", MessType.Warning);
                            ui.AddToLog("Edit setting =>master_IP", MessType.Warning);
                            break;
                        }
                }
                break;
            case Role.Slave:
                tasker?.Stop("SetRole");
                udp_master?.Dispose();
                udp_bot = new UdpBot();
                tasker = new Slave();
                break;
            case Role.None:
                tasker?.Stop("SetRole");
                tasker = new None();
                udp_master?.Dispose();
                udp_bot?.Dispose();
                break;
            default:
                ui.AddToLog("SetRole err: not present role=[" + sett.role + "]", MessType.Critical);
                break;
        }
    }
}