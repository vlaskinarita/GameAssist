using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using ImGuiNET;
using static System.Runtime.InteropServices.JavaScript.JSType;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;
public partial class Entity : RemoteObjectBase {
    /// <summary>
    ///     Initializes a new instance of the <see cref="Entity" /> class.
    ///     NOTE: Without providing an address, only invalid and empty entity is created.
    /// </summary>
    internal Entity() : base(IntPtr.Zero) {
       componentAddresses = new();
       componentCache = new();
       Path = string.Empty;
       id = 0;
       IsValid = false;
       eType = eTypes.Unidentified;
       isnearby = false;
    }

    internal Entity(IntPtr address) : this() {
        this.Address = address;
    }
    internal override void Tick(IntPtr ptr, string from=null) {
        Address = ptr;
        if (Address == IntPtr.Zero)
            return;
        var data = ui.m.Read<EntityOffsets>(this.Address);
        var cl_ptr = data.ItemBase.ComponentListPtr; //component list_ptr
        IsValid = EntityHelper.IsValidEntity(data.IsValid);
        if (!IsValid) {
            // Invalid entity data is normally corrupted. let's not parse it.
            return;
        }
        id = data.Id;
    
        if (eType == eTypes.Useless) {
            // let's not read or parse any useless entity components.
            return;
        }

        if (cl_ptr.TotalElements(1) <= 0 && cl_ptr.TotalElements(1) >100 ) {
            ui.AddToLog(tName + ".Tick err comp_ptr", MessType.Error);
            return;
        }
        var need_parce = false;
        var cch = cl_ptr.GetHashCode();
        if (cch != last_comp_hash) {
            UpdateComponentData(data.ItemBase);
            last_comp_hash = cch;
            need_parce = true;
        }
        if (last_ptr != Address || need_parce) {
            ParseEntityType();
            last_ptr = Address;
        }
        foreach (var kv in componentCache) {
            kv.Value.Tick(kv.Value.Address);
        }
        if (id == 1331) {
        }
    }
    IntPtr last_ptr = default;
    int last_comp_hash = 0;
   
    /// <summary>
    ///     Updates the component data associated with the Entity base object (i.e. item).
    /// </summary>
    /// <param name="idata">Entity base (i.e. item) data.</param>
    /// <param name="hasAddressChanged">has this class Address changed or not.</param>
    protected void UpdateComponentData(ItemStruct idata) {
        this.componentAddresses.Clear();
        this.componentCache.Clear();
        var entityComponent = ui.m.ReadStdVector<IntPtr>(idata.ComponentListPtr);
        var entityDetails = ui.m.Read<EntityDetails>(idata.EntityDetailsPtr);
        if (!ui.std_wstrings.ContainsKey(entityDetails.name))
           ui.std_wstrings[entityDetails.name] = ui.m.ReadStdWString(entityDetails.name);
        Path = ui.std_wstrings[entityDetails.name];
        var lookupPtr = ui.m.Read<ComponentLookUpStruct>(entityDetails.ComponentLookUpPtr);

        var namesAndIndexes = ui.m.ReadStdBucket<ComponentNameAndIndexStruct>(
            lookupPtr.ComponentsNameAndIndex);
        for (var i = 0; i < namesAndIndexes.Count; i++) {
            var nameAndIndex = namesAndIndexes[i];
            if (nameAndIndex.Index >= 0 && nameAndIndex.Index < entityComponent.Length) {
                var name = ui.m.ReadString(nameAndIndex.NamePtr);
                if (!string.IsNullOrEmpty(name)) {
                    this.componentAddresses.TryAdd(name, entityComponent[nameAndIndex.Index]);
                }
            }
        }
    }
    bool isnearby;
    public bool IsNearby => this.IsValid && this.isnearby;
    /// <summary>
    ///     Gets a value indicating whether this entity can explode or not.
    /// </summary>
    public bool CanExplode =>
        this.eType == eTypes.Monster ||
        this.eType == eTypes.Useless ||
        this.eType == eTypes.Stage1RewardFIT ||
        this.eType == eTypes.Stage1FIT ||
        this.eType == eTypes.Stage1EChestFIT;
    /// <summary>
    ///     Calculate the distance from the other entity.
    /// </summary>
    /// <param name="other">Other entity object.</param>
    /// <returns>
    ///     the distance from the other entity
    ///     if it can calculate; otherwise, return 0.
    /// </returns>
    public int DistanceFrom(Entity other) {
        if (this.GetComp<Render>(out var myPosComp) &&
            other.GetComp<Render>(out var otherPosComp)) {
            var dx = myPosComp.gpos_f.X - otherPosComp.gpos_f.X;
            var dy = myPosComp.gpos_f.Y - otherPosComp.gpos_f.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        // Console.WriteLine($"Render Component missing in {this.Path} or {other.Path}");
        return 0;
    }
    string _metadata;
    public string Metadata {
        get {
            if (_metadata == null) {
                if (Path != null) {
                    var splitIndex = Path.IndexOf("@", StringComparison.Ordinal);

                    if (splitIndex != -1)
                        _metadata = Path.Substring(0, splitIndex);
                    else
                        return Path;
                }
            }

            return _metadata;
        }
    }
    public Rarity rarity {
        get {
            if (GetComp<ObjectMagicProperties>(out var omp)) {
                if (omp != null)
                    return omp.Rarity;
            }
            return Rarity.Normal;
        }
    }

    public Entity target;
    Dictionary<GameStat, int> _stats = null;
    public Dictionary<GameStat, int> Stats {
        get {
            if (GetComp<Stats>(out var stats))
                _stats = stats.stats;
            return _stats;
        }
    }
    public override string ToString() {
        return this.eType.ToString() +" id="+id;
    }
    Buffs _buffs;
    public Buffs buffs {
        get {
            if (GetComp<Buffs>(out _buffs) && _buffs!=null)
                return _buffs;
            return null;
        }
    }

    string _renderName = "NoName";
    public string RenderName {
        get {
            if (GetComp<Render>(out var render) && render != null
                && render.owner_ptr==this.Address) {
                _renderName = render.Name;
                return _renderName;
            }
            return _renderName;
        }
    }
    bool _isTargetable;
    public bool IsTargetable {
        get {
            if ( GetComp<Targetable>(out var _targ) && _targ != null 
                        && _targ.owner_ptr == Address) {
                _isTargetable = _targ.isTargetable;
                return _isTargetable;
            }
            return _isTargetable;
        }
    }

    bool _hidden;
    public bool IsHidden {
        get {
            if (GetComp<Buffs>(out var buffs)) {
                _hidden = buffs != null && buffs.StatusEffects.ContainsKey("hidden_monster");
            }
            return _hidden;
        }
    }
    public bool IsFriendly {
        get {
            if (GetComp<Positioned>(out var _pos)) {
                return _pos != null && _pos.IsFriendly;
            }
            return false;
        }
    }
    bool _is_opened;
    public bool IsOpened {
        get {
            if (GetComp<Chest>(out var chest) && chest != null && chest.owner_ptr == Address) {
                if (GetComp<Targetable>(out var trg) && trg != null && trg.owner_ptr == Address) {
                    _is_opened = trg.isTargeted || chest.IsOpened;
                }
                _is_opened = chest.IsOpened;
            }
            return _is_opened;
        }
    }

    public float danger_summ = 0f;
    public float danger_k = 1f;
    public float danger_rt => danger * danger_k; //real time
    float _bd = -1f; //base danger. set -1 for calculate ones
    public float danger {
        get {
            if (IsDead || IsHidden) {
                return 0;
            }
            else {
                if (_bd == -1f) { //not calculated yet
                    if ((Path.Contains("Daemon") && GetComp<DiesAfterTime>(out var _))
                        || eType != eTypes.Monster || IsFriendly) {
                        _bd = 0;
                        return _bd;
                    }
                    _bd = 1f; //base danger
                    if (GetComp<Actor>(out var actor)) {
                        if (actor.Action == ActionFlags.Dead)//|| actor.Animation == AnimationE.Death
                        {
                            _bd = 0;
                            return _bd;
                        }
                    }
                    switch (this.rarity) {
                        case Rarity.Normal:
                            _bd = 0.33f;
                            break;
                        case Rarity.Magic:
                            _bd = 1f;
                            break;
                        case Rarity.Rare:
                            _bd = 3f;
                            break;
                        case Rarity.Unique:
                            _bd = 10f;
                            break;
                    }
                    if (Path.Contains("Necromancer")) {
                        //Debug.Assert( actor.ActorSkills.Any(s => s.Name == "NecromancerReviveSkeleton"));
                        _bd = 10f;
                    }
                    if (GetComp<ObjectMagicProperties>(out var omp)) {
                        if (omp.Mods.Count > 2) _bd = 3f;
                        if (omp.Mods.Count > 3) _bd = 5f;
                        if (omp.Mods.Count > 5) _bd = 10f;
                        if (omp.Mods.Any(m => m.Contains("FastRun") || m.Contains("Bloodlines")
                        || m.Contains("CorruptedBlood") || m.Contains("MonsterAura")
                        || m.Contains("RareMonsterPack") || m.Contains("CannotBeStunned")
                        || m.Contains("MonsterMapBoss")))
                            _bd = 10f;
                    }
                    _bd = (float)Math.Round(_bd, 2);
                }
                return _bd;
            }
        }
    }
    public bool IsDead => !IsAlive;
    public bool IsAlive {
        get {
            if (this.GetComp<Life>(out var life) ) {
                return life.Health.Current > 0; 
            }
            return false;
        }
    }
    V3 _pos;
    public V3 pos {
        get {
            if (GetComp<Render>(out var render)) {
                //Debug.Assert(render.owner_ptr == Address);
                _pos.X = render.WorldPosition.X;
                _pos.Y = render.WorldPosition.Y;
                _pos.Z = render.WorldPosition.Z + render.ModelBounds.Z;
                return _pos;
            }
            return default;
        }
    }
    public V2 gpos_f {
        get {
            if (GetComp<Render>(out var render)
                && render != null && render.owner_ptr == Address) {
                    return render.gpos_f;
            }
            return V2.Zero;
        }
    }
    public V2 gpos {
        get {
            if (GetComp<Positioned>(out var positioned)
                && positioned !=null && positioned.owner_ptr == Address) {
                    return positioned.GridPos;
            }
            return V2.Zero;
        }
    }

    //public V2 gpos {
    //    get {
    //        if (IsValid && GetComp<Render>(out var render))
    //            if (render.owner_ptr == Address)
    //                return render.GridPosition;
    //        return V2.Zero;
    //    }
    //}
    /// <summary>
    /// in the Grid units
    /// </summary>
    public float gdist_to_me {
        get {
            var plaer = ui.me;
            if (plaer != null && plaer.gpos != V2.Zero) {
                //var gdist = plaer.gpos.GetDistance(gpos);
                var fdist = plaer.gpos.GetDistance(gpos);
                //Debug.Assert(gdist.Round(0)== fdist.Round(0));
                return fdist;
            }
            return float.MaxValue;
        }
    }
    uint _key;
    public uint GetKey {
        get {
            if (_key == 0) {
                var sb = new StringBuilder();
                foreach (var c in componentAddresses )
                    sb.Append(c.Key + "_");
                string pos = "Zero";
                if (GetComp<Render>(out var rend) && !rend.b_can_move)
                    pos = rend.WorldPosition.ToByte().ToHexString();
                sb.Append(pos);
                _key = sb.ToString().ToUint32Hash();
            }
            return _key;
        }
    }

    static List<string> diesAfterTimeIgnore = new()
    {
            "Metadata/Monsters/AtlasExiles/CrusaderInfluenceMonsters/CrusaderArcaneRune",
            "Metadata/Monsters/Daemon/DaemonLaboratoryBlackhole",
            "Metadata/Monsters/AtlasExiles/AtlasExile",
            "Metadata/Monsters/Daemon/MaligaroBladeVortexDaemon",
            "Metadata/Monsters/Daemon/DoNothingDaemon",
            "Metadata/Monsters/Daemon/ShakariQuicksandDaemon",
            "Metadata/Monsters/AtlasInvaders/CleansingMonsters/CleansingPhantasmPossessionDemon",
            "Metadata/Monsters/Daemon/Archnemesis"
        };

    static string deliriumHiddenMonsterStarting =
        "Metadata/Monsters/LeagueAffliction/DoodadDaemons/DoodadDaemon";

    public readonly ConcurrentDictionary<string, IntPtr> componentAddresses;// 0x00000211e0d14810
    readonly ConcurrentDictionary<string, RemoteObjectBase> componentCache;

   

    /// <summary>
    ///     Gets the Path (e.g. Metadata/Character/int/int) assocaited to the entity.
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    ///     Gets the Id associated to the entity. This is unique per map/Area.
    /// </summary>
    public uint id { get; private set; }

    /// <summary>
    ///     Gets or Sets a value indicating whether the entity
    ///     exists in the game or not.
    /// </summary>
    public bool IsValid { get; set; }
    /// <summary>
    ///     Gets a value indicating the type of entity this is.
    /// </summary>
    public eTypes eType { get; protected set; }

    /// <summary>
    ///     Gets the Component data associated with the entity.
    /// </summary>
    /// <typeparam name="T">Component type to get.</typeparam>
    /// <param name="component">component data.</param>
    /// <returns>true if the entity contains the component; otherwise, false.</returns>
    public bool GetComp<T>(out T component) where T : RemoteObjectBase {
        component = null;
        var componenName = typeof(T).Name;
        if (this.componentCache.TryGetValue(componenName, out var comp)) {
            component = (T)comp;
            return true;
        }

        if (this.componentAddresses.TryGetValue(componenName, out var compAddr)) {
            if (compAddr != IntPtr.Zero) {
                component = Activator.CreateInstance(typeof(T), compAddr) as T;
                if (component != null) {
                    component.Tick(component.Address);
                    componentCache[componenName] = component;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    ///     Converts the <see cref="Entity" /> class data to ImGui.
    /// </summary>
    internal override void ToImGui() {
        base.ToImGui();
        ImGui.Text($"Path: {this.Path}");
        ImGui.Text($"Id: {this.id}");
        ImGui.Text($"Is Valid: {this.IsValid}");
        ImGui.Text($"Entity Type: {this.eType}");
        if (ImGui.TreeNode("Components")) {
            foreach (var kv in this.componentAddresses) {
                if (this.componentCache.ContainsKey(kv.Key)) {
                    if (ImGui.TreeNode($"{kv.Key}")) {
                        this.componentCache[kv.Key].ToImGui();
                        ImGui.TreePop();
                    }
                }
                else {
                    ImGuiExt.IntPtrToImGui(kv.Key, kv.Value);
                }
            }

            ImGui.TreePop();
        }
    }

    protected override void Clear() {
        this.componentAddresses?.Clear();
        this.componentCache?.Clear();
    }
}