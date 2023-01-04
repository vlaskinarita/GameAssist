using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Stas.GA;
public class UdpBot : IDisposable {//version=1
    string tname => this.GetType().Name;
    Socket socket;
    public bool b_screening, b_m_pointer = true, b_debug = false;
    Stopwatch sw = new Stopwatch();
   
    /// <summary>
    /// thro new connection if was an error
    /// </summary>
    bool b_need_remake = true;
    bool was_disposed = false;
    public UdpBot() {
        byte[] data = new byte[1024];
        int b_count;
        var thred = new Thread(() => {
            while (was_disposed) {
                try {
                    if (b_need_remake) {
                        b_need_remake = false;
                        var err = false;
                        socket ??= new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        switch (ui.sett.master_name) {
                            case MasterNams.MANA:
                                socket.Connect(IPAddress.Parse("192.168.1.10"), ui.sett.master_port);
                                break;
                            case MasterNams.LARS:
                                socket.Connect(IPAddress.Parse("192.168.1.13"), ui.sett.master_port);
                                break;
                            case MasterNams.DAMAGE:
                                socket.Connect(IPAddress.Parse("192.168.1.7"), ui.sett.master_port);
                                break;
                            case MasterNams.VOVA:
                                socket.Connect(IPAddress.Parse("192.168.1.15"), ui.sett.master_port);
                                break;
                            case MasterNams.CURSE:
                                socket.Connect(IPAddress.Parse("192.168.1.11"), ui.sett.master_port);
                                break;
                            case MasterNams.GF1030: //18-C0-4D-E4-B9-99
                                socket.Connect(IPAddress.Parse("192.168.1.2"), ui.sett.master_port);
                                break;
                            default:
                                err = true;
                                ui.AddToLog("UdpBot err dont have  right setting to connect to[" + ui.sett.master_name.ToString() + "]", MessType.Critical); break;
                        }
                        if (!err) {
                            var ep = (IPEndPoint)socket.LocalEndPoint;
                            ui.bi = new BotInfo(ep.Address, ep.Port);
                        }
                    }
                    b_count = socket.Receive(data, 0); //last packet
                    var opc = (Opcode)data[0];
                    #region Decode
                    switch (opc) {
                        case Opcode.MouseRightClick: {
                                 break;
                                }
                              
                        case Opcode.LinkMe: {
                               
                                break;
                            }
                        case Opcode.SetTarget: {
                               
                                break;
                            }
                        case Opcode.NavGo: {
                             
                                break;
                            }
                        case Opcode.ResurectCheckPOint: {
                              
                                break;
                            }
                        case Opcode.ResurectTown: {
                               
                                break;
                            }
                        case Opcode.UseTotem: {
                             
                                break;
                            }
                        case Opcode.Jump: {
                              
                                break;
                            }
                        case Opcode.Exit:
                            Environment.Exit(1);
                            break;
                        case Opcode.FallowHard:
                           
                            break;
                        case Opcode.TpToLeader:
                         
                            break;
                        case Opcode.Transit: {
                               
                            }
                            break;
                        case Opcode.FocusGP: {
                               
                                break;
                            }
                        case Opcode.SetUltim: {
                               
                                break;
                            }
                        case Opcode.UseFlare: {
                               
                                break;
                            }
                        case Opcode.UseTNT: {
                              
                            }
                            break;

                        case Opcode.ResetState:
                          
                            break;
                        case Opcode.Ping: {
                                ui.leader.lfn.FillFromByteArray(data, 1);
                                ui.sett.last_leader_name = ui.leader.lfn.leader_name;
                            }
                            break;
                        case Opcode.Test:
                            //ui.bi.b_test = !ui.bi.b_test;
                            //ui.tasker.Test();
                            break;
                        case Opcode.UseChest:
                            ui.bi.use_chest = !ui.bi.use_chest;
                            break;
                        case Opcode.UseLoot:
                            ui.bi.use_loot = !ui.bi.use_loot;
                            break;
                        case Opcode.UnHold:
                            ui.tasker.Unhold();
                            break;
                        case Opcode.Hold:
                            ui.tasker.Hold();
                            break;
                        case Opcode.SetLeader: {
                            }
                            break;
                        case Opcode.RestartUdp:
                            break;
                        case Opcode.CleareErr:
                            ui.bi.have_error = false;
                            File.WriteAllText(ui.sett.log_fname, String.Empty);
                            break;
                        case Opcode.Looting:
                            //ui.my_state?.UpdateMode(Mode.looting);
                            break;
                        case Opcode.BotRoleList:
                            //Reply(Opcode.BotRoleList, JSON.ToByte(ui.cb_roles.Items));
                            break;
                        case Opcode.KeyPress: {
                                var key = GetKey();
                                if (ui.b_game_top) {
                                    if (key == Keys.E) {
                                        if (ui.worker.b_use_low_life)
                                            ui.tasker.UseManaFlask();
                                        else
                                            ui.tasker.UseLifeFlask();
                                    }
                                    else
                                        Keyboard.KeyPress(key);
                                    if (b_debug)
                                        ui.AddToLog("KeyPress [" + key + "]");
                                }
                                break;
                            }
                        case Opcode.KeyUp: {
                                var key = GetKey();
                                if (ui.b_game_top) {
                                    Keyboard.KeyUp(key);
                                    if (b_debug) ui.AddToLog("KeyUP [" + key + "]");
                                }
                            }
                            break;
                        case Opcode.KeyDown: {
                                var key = GetKey();
                                if (ui.b_game_top) {
                                    Keyboard.KeyDown(key);
                                    if (b_debug) ui.AddToLog("KeyDown [" + key + "]");
                                }
                            }
                            break;
                        case Opcode.MouseLeftClick: {
                                if (ui.b_game_top)
                                    Mouse.LeftClick("Opcode.MouseLeftClick");
                            }
                            break;
                        case Opcode.OpenTrade:
                            //Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.MENU }, VirtualKeyCode.VK_D);
                            break;
                        case Opcode.MouseScroll: {
                                var sv = BitConverter.ToInt32(data, 1);
                                Mouse.VerticalScroll(sv > 0);
                            }
                            break;
                        case Opcode.MouseLeftDown:
                            Mouse.LeftDown("Opcode.MouseLeftDown");
                            break;
                        case Opcode.MouseLeftUp:
                            Mouse.LeftUp("Opcode.MouseLeftUp");
                            break;
                        case Opcode.Server_down: {
                                ui.AddToLog("Server will DOWN", MessType.Warning);
                                break; //return?
                            }
                        //case Opcode.UseMacros: { //blocking data recive
                        //        var key = data.To_UTF8_String(1);
                        //        await ui.worker.UseMacros(key);
                        //        break;
                        //    }
                        //case Opcode.BuffUp: //blocking data recive
                        //    await ui.worker.UseMacros("F9");
                        //    //await ui.worker.MouseBuffs();
                        //    //await ui.worker.SetLastBuff();
                        //    break;
                        case Opcode.StopAll:
                            ui.worker.ReliseKeys();
                            break;
                        case Opcode.SavingAss:
                            ui.worker.SavingAss();
                            break;
                        case Opcode.StartMoving:
                            ui.worker.StartMoving("Opcode.StartMoving");
                            break;
                        case Opcode.StopMoving:
                            ui.worker.StopMoving("Opcode.StopMoving");
                            break;
                        default:
                            ui.AddToLog("unknow opc: " + opc);
                            break;
                    }
                    #endregion
                }
                catch (Exception ex) {
                    b_need_remake = true;
                    var err_mess = ex.Message;
                    if (ex.Message.StartsWith("Once the socket has been disconnected", StringComparison.Ordinal)
                    || ex.Message.StartsWith("An existing connection was forcibly closed", StringComparison.Ordinal) 
                    || ex.Message.Contains("A blocking operation was interrupted by", StringComparison.Ordinal))
                        err_mess = "Can't connect to master server=[" + ui.sett.master_name + "]";
                    ui.AddToLog(tname + "... " + err_mess, MessType.Error);
                    Thread.Sleep(5000);
                }
                //ui.AddToLog(tname + " elapse=["+ (elapse +=1)+ "]");
                Thread.Sleep(1);
            }
        });
        //thred.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
        thred.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        thred.IsBackground = true;
        //thred.Start();

        Keys GetKey() {
            var res = (Keys)BitConverter.ToInt32(data, 1);
            return res;
        }
    }

    public void Send(Opcode opc, byte[] data) {
        var ba = BYTE.Concat(new byte[] { (byte)opc }, data);
        try {
            socket?.Send(ba);
        }
        catch (Exception ex) {
            ui.AddToLog(tname + "=>Send... " + ex.Message);
        }
    }

    public void Dispose() {
        try {
            was_disposed = true;
            socket?.Shutdown(SocketShutdown.Both);
            socket?.Close();
            socket?.Dispose();
            socket = null;
        }
        catch (Exception ex) {
            ui.AddToLog(this.GetType().Name + " err: " + ex.Message, MessType.Error);
        }
    }
}
