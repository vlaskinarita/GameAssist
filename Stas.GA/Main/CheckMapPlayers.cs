using System.Diagnostics;
using System.Reflection;
namespace Stas.GA;

public partial class ui {
    static void CheckMapPlayers() {
        Entity found = null;
        string new_leader_name = "Unknow";
        var players = server_data.nearest_players;
        ////var players = entities.Where(e => e.eType == eTypes.OtherPlayer 
        //                                && e.gdist_to_me < sett.max_entyty_valid_gdistance)
        //                                .OrderBy(e => e.gdist_to_me).ToList();
        Entity new_link = null;
        foreach (var pl in players) {
            //if I(for bot only mb) must use link
            if (worker?.link != null && !string.IsNullOrEmpty(worker.link.l_name)) {
                if (pl.Name == worker.link.l_name) {
                    new_link = new Entity( pl.owner_ptr);
                }
            }
            if (pl.Name != leader.lfn.leader_name)
                continue;
            if (pl.Name == sett.last_leader_name) {
                found = new Entity(pl.owner_ptr);
                new_leader_name = pl.Name;
                break;
            }
            if (found != null)
                break;
        }
        if (found == null) {
            leader.ent = null;
        }
        else {
            if (leader.ent.Address != found.Address) {
                leader.ent = found;
                AddToLog("the Leader was changed to: " + new_leader_name + " [" + found.id + "]");
            }
        }
        if (worker?.link != null) {
            if (new_link == null) {
                if (leader.ent != null) {
                    curr_link = leader.ent;
                }
            }
            else if (curr_link == null || curr_link.Address != new_link.Address)
                curr_link = new_link;
        }
    }
}
