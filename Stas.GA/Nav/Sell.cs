using Newtonsoft.Json;
using V2 = System.Numerics.Vector2;

namespace Stas.GA {
    public class Cell :IEquatable<Cell> {
        public string info; //for debug ui safe zones
        public bool b_block = false;
        [JsonIgnore]
        public int id { get; set; }
        public GridCell root;
        public Cell Parent;
        public float DistanceToTarget = -1;
        [JsonIgnore]
        public V2 a => min;
        public V2 b;
        [JsonIgnore]
        public V2 c => max;
        public V2 d;
        public bool b_tested;
        public bool b_visited;
        public bool b_ncollected; //neighbor_collected;
        [JsonIgnore]
        public V2 center => (min + max) * 0.5f;
        public string debug;
        [JsonIgnore]
        public int width =>( int)(max.X - min.X);
        [JsonIgnore]
        public int height => (int)(max.Y - min.Y);
        public float area;
        [JsonIgnore]
        public float gdist_to_me => ui.me==null?float.MaxValue: center.GetDistance(ui.me.gpos);
        V2 _min;
       
        public V2 min { get { return _min; } 
            set { _min = value; 
                b = new V2(_min.X, _max.Y);
                d = new V2(_max.X, _min.Y);
                area = (max.X - min.X) * (max.Y - min.Y);
            } }
        V2 _max;
       
        public V2 max {
            get { return _max; }
            set {
                _max = value;
                b = new V2(_min.X, _max.Y);
                d = new V2(_max.X, _min.Y);
                area = (max.X - min.X) * (max.Y - min.Y);
            }
        }
        public Cell(float min_x, float min_y, float max_x, float max_y) {
            min = new V2(min_x, min_y);
            max = new V2(max_x, max_y);
            area = (max.X - min.X) * (max.Y - min.Y);
        }
        [JsonConstructor]
        public Cell(V2 min, V2 max) {
            this.min = min;
            this.max = max;
            area = (max.X - min.X) * (max.Y - min.Y);
        }
        [JsonIgnore]
        public float F {
            get {
                if(DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public float Cost = -1;
        public override bool Equals(Object obj) {
            Cell cell = obj as Cell;
            return Equals(cell);
        }

        public bool Equals(Cell cell) {
            return a == cell.a && b == cell.b && c == cell.c && d == cell.d;
        }

        public override int GetHashCode() {
            return id.GetHashCode();
        }

        public bool Contains(float x, float y) {
            return x >= min.X && x <= max.X && y >= min.Y && y <= max.Y;
        }
        public bool Contains(V2 p) {
            return p.X >= min.X && p.X <= max.X && p.Y >= min.Y && p.Y <= max.Y;
        }
        public bool Insade(V2 p) {
            return p.X > min.X && p.X < max.X && p.Y > min.Y && p.Y < max.Y;
        }
        /// <summary>Determines whether a specified rectangle intersects with this rectangle.</summary>
        /// <param name="value">The Cell to evaluate.</param>
        public bool Overlaps(Cell trg) {
            return (max.X >= trg.min.X && min.X <= trg.max.X && max.Y >= trg.min.Y && min.Y <= trg.max.Y) ||
                    (max.X > trg.min.X && min.X < trg.max.X && max.Y > trg.min.Y && min.Y < trg.max.Y);
        }
       
        public List<Cell> neighbours = new List<Cell>();

        public float Distance(Cell cell) {
            return center.GetDistance(cell.center);
        }

        public float Distance(V2 p) {
            return (float)Math.Sqrt(DistanceSqr(p));
        }

        public float DistanceSqr(V2 p) {
            float dx = Math.Max(0, Math.Max(min.X - p.X, p.X - max.X));
            float dy = Math.Max(0, Math.Max(min.Y - p.Y, p.Y - max.Y));
            return dx * dx + dy * dy ;
        }

        public class CompareById :IComparer<Cell> {
            public int Compare(Cell x, Cell y) {
                return x.id.CompareTo(y.id);
            }
        }
   
        public List<V2> LS_intersection(  V2 ls_start, V2 ls_end) { //working well
            return default;
        }
        internal static int LastCellGlobalId = 0;
        public override string ToString() {
            return "id="+id + " min="+min.ToIntString() + " max=" + max.ToIntString();
        }
    }

    class VisitedGrid {
        public float min_x { get; set; }
        public float min_y { get; set; }
        public float max_x { get; set; }
        public float max_y { get; set; }

        public List<VisitedCell> visited { get; set; }
        public VisitedGrid() { }
        public VisitedGrid(GridCell c) {
            min_x = c.min.X; min_y = c.min.Y; max_x = c.max.X; max_y = c.max.Y;
            visited = new List<VisitedCell>();
        }

        public override string ToString() {
            return visited.Count.ToString();
        }
    }

    class VisitedCell {
        public float min_x { get; set; }
        public float min_y { get; set; }
        public float max_x { get; set; }
        public float max_y { get; set; }

        public VisitedCell() { }
        public VisitedCell(Cell c) {
            min_x = c.min.X; min_y = c.min.Y; max_x = c.max.X; max_y = c.max.Y;
        }
    }
}
