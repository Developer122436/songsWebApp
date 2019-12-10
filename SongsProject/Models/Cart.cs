using System.Collections.Generic;
using System.Linq;

namespace SongsProject.Models {

    public class Cart {

        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Song Song, int quantity) {
            CartLine line = lineCollection
            .FirstOrDefault(p => p.Song.Id == Song.Id);
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Song = Song,
                    Quantity = quantity
                });

            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Song Song) =>
        lineCollection.RemoveAll(l => l.Song.Id == Song.Id);

        public virtual decimal ComputeTotalValue() =>
        lineCollection.Sum(e => e.Song.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine {
        public int CartLineID { get; set; }
        public Song Song { get; set; }
        public int Quantity { get; set; }
    }
}