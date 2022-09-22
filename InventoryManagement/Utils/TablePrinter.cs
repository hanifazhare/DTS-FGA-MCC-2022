using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Utils.TablePrinter {
    public class TablePrinter {
        private readonly string[] titles;
        private readonly List<int> lengths;
        private readonly List<string[]> rows = new List<string[]>();

        public TablePrinter(params string[] titles) {
            this.titles = titles;
            lengths = titles.Select(t => t.Length).ToList();
        }

        public void AddRow(params object[] row) {
            if (row.Length != titles.Length) {
                throw new System.Exception($"Added row length [{row.Length}] is not equal to title row length [{titles.Length}]");
            }
            rows.Add(row.Select(o => o.ToString()).ToArray());
            for (int i = 0; i < titles.Length; i++) {
                if (rows.Last()[i].Length > lengths[i]) {
                    lengths[i] = rows.Last()[i].Length;
                }
            }
        }

        public void Print() {
            lengths.ForEach(l => Console.Write("+-" + new string('-', l) + '-'));
            Console.WriteLine("+");

            string line = "";
            for (int i = 0; i < titles.Length; i++) {
                line += "| " + titles[i].PadRight(lengths[i]) + ' ';
            }
            Console.WriteLine(line + "|");

            lengths.ForEach(l => Console.Write("+-" + new string('-', l) + '-'));
            Console.WriteLine("+");

            foreach (var row in rows) {
                line = "";
                for (int i = 0; i < row.Length; i++) {

                    /*
                     * Numbers are padded to the left
                     * If you want that, you can uncomment below
                     */
                    // if (double.TryParse(row[i], out double n)) {
                    //     line += "| " + row[i].PadLeft(lengths[i]) + ' ';
                    // } else {
                    //     line += "| " + row[i].PadRight(lengths[i]) + ' ';
                    // }

                    /*
                     * All padded to the right
                     */
                    line += "| " + row[i].PadRight(lengths[i]) + ' ';
                }
                Console.WriteLine(line + "|");
            }

            lengths.ForEach(l => Console.Write("+-" + new string('-', l) + '-'));
            Console.WriteLine("+");
        }
    }
}