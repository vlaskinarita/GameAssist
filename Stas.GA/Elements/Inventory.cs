using System.Collections.Concurrent;
using ImGuiNET;
namespace Stas.GA;

/// <summary>
///     Knows how to parse player, NPC, Crafting, stash inventories available in ServerData
///     and get the items available in them.
/// </summary>
public class Inventory : Element {
    /// <summary>
    ///     This array stores items addresses in a given inventory.
    ///     Items addresses are in order w.r.t inventory slots. There might be duplicates or IntPtr.Zero
    ///     in case an item holds 2 slots or there is no item in the slot respectively.
    /// </summary>
    private IntPtr[] itemsToInventorySlotMapping;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Inventory" /> class.
    /// </summary>
    /// <param name="address">address of the remote memory object.</param>
    /// <param name="name">name of the inventory for displaying purposes.</param>
    internal Inventory(IntPtr address, string name) : base(address, name) {

    }
    internal override void Tick(IntPtr ptr, string from) {
        base.Tick(ptr, from);
        if (Address == IntPtr.Zero)
            return;
        var invInfo = ui.m.Read<InventoryStruct>(Address);
        TotalBoxes = invInfo.TotalBoxes;
        ServerRequestCounter = invInfo.ServerRequestCounter;
        itemsToInventorySlotMapping = ui.m.ReadStdVector<IntPtr>(invInfo.ItemList);
        Items.Clear();
        var list = itemsToInventorySlotMapping.Distinct();
#if DEBUG
        foreach (var invItemPtr in list)
            run(invItemPtr);
#else
        Parallel.ForEach(list, invItemPtr => {
            run(invItemPtr);
        });
#endif

        void run(IntPtr invItemPtr) {
            if (invItemPtr != IntPtr.Zero) {
                var invItem = ui.m.Read<InventoryItemStruct>(invItemPtr);
                if (Items.ContainsKey(invItemPtr)) {
                    Items[invItemPtr].Tick(invItem.Item);
                }
                else {
                    var item = new Item(invItem.Item);
                    if (!string.IsNullOrEmpty(item.Path)) {
                        if (!Items.TryAdd(invItemPtr, item)) {
                            ui.AddToLog(tName + "Failed to add item into the Inventory Item Dict.", MessType.Error);
                        }
                    }
                }
            }
        }
        foreach (var item in Items) {
            if (!item.Value.IsValid) {
                Items.TryRemove(item.Key, out _);
            }
        }
    }
    protected override void Clear() {
        TotalBoxes = default;
        ServerRequestCounter = default;
        itemsToInventorySlotMapping = null;
        Items.Clear();
    }

    public IList<NormalInventoryItem> VisibleInventoryItems { get; private set; }
    /// <summary>
    ///     Gets a value indicating total number of boxes in the inventory.
    /// </summary>
    public StdTuple2D<int> TotalBoxes { get; private set; }

    /// <summary>
    ///     Gets a value indicating total number of requests send to the server for this inventory.
    /// </summary>
    public int ServerRequestCounter { get; private set; }

    /// <summary>
    ///     Gets all the items in the inventory.
    /// </summary>
    public ConcurrentDictionary<IntPtr, Item> Items { get; } =  new();

    /// <summary>
    ///     Gets the item at the specific slot in the inventory.
    ///     Always check if the returned item IsValid or not by comparing
    ///     Item Address with IntPtr.Zero.
    /// </summary>
    /// <param name="y">Inventory slot row, starting from 0.</param>
    /// <param name="x">Inventory slot column, starting from 0.</param>
    /// <returns>Item on the given slot.</returns>
    [SkipImGuiReflection]
    public Item this[int y, int x] {
        get {
            if (y >= TotalBoxes.Y || x >= TotalBoxes.X) {
                return new Item(IntPtr.Zero);
            }

            var index = y * TotalBoxes.X + x;
            if (index >= itemsToInventorySlotMapping.Length) {
                return new Item(IntPtr.Zero);
            }

            var itemAddr = itemsToInventorySlotMapping[index];
            if (itemAddr == IntPtr.Zero) {
                return new Item(IntPtr.Zero);
            }

            if (Items.TryGetValue(itemAddr, out var item)) {
                return item;
            }

            return new Item(IntPtr.Zero);
        }
    }

    /// <inheritdoc />
    internal override void ToImGui() {
        base.ToImGui();
        ImGui.Text($"Total Boxes: {TotalBoxes}");
        ImGui.Text($"Server Request Counter: {ServerRequestCounter}");
        if (ImGui.TreeNode("Inventory Slots")) {
            for (var y = 0; y < TotalBoxes.Y; y++) {
                var data = string.Empty;
                for (var x = 0; x < TotalBoxes.X; x++) {
                    if (itemsToInventorySlotMapping[y * TotalBoxes.X + x] != IntPtr.Zero) {
                        data += " 1";
                    }
                    else {
                        data += " 0";
                    }
                }

                ImGui.Text(data);
            }

            ImGui.TreePop();
        }

        if (ImGui.TreeNode("Items")) {
            foreach (var item in Items) {
                if (ImGui.TreeNode($"{item.Value.Path}##{item.Value.Address.ToInt64()}")) {
                    item.Value.ToImGui();
                    ImGui.TreePop();
                }
            }

            ImGui.TreePop();
        }
    }

   
   

}
