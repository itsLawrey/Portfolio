using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleGame.Persistence // TODO: save player state in file
{
    public class BlackholeFileDataAccess : IBlackholeDataAccess
    {
        public async Task<BlackholeTable> LoadAsync(string path)
        {
            try
            {
                BlackholeTable game = new BlackholeTable();
                using var f = File.OpenText(path);
                string? line = await f.ReadLineAsync();

                int coord = -1;
                if(line != null)
                {
                    var temp = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);//temp ben elemezzuk hogy van e selected cell 
                    game.Player = Enum.Parse<CellState>(temp[0]);//jatekos kiolvasasa

                    if (temp.Length > 1)
                    {
                        coord = int.Parse(temp[1]);
                    }


                }
                
                line = await f.ReadLineAsync();
                int index = 0;
                if (line !=null)
                {
                    string[] cellStates = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    game.TableSize = cellStates.Length;
                    while (line != null && line.Trim() != "") //Ha nem null vagy nem csak ures sor
                    {
                        cellStates = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < cellStates.Length; i++)
                        {
                            game.GenerateCell(index, i, Enum.Parse<CellState>(cellStates[i]));
                        }
                        index++;
                        line = await f.ReadLineAsync();
                    }
                }
                if (coord > -1)
                {
                    game.SelectCell(coord / 10, coord % 10, true); //ha volt selected adat a fileban, akkor ez beallitja a jatektablan azt a mezot kivalasztottra
                }
                return game;
            }
            catch
            {
                throw new BlackholeDataException();
            }
        }

        public async Task SaveAsync(string path, BlackholeTable table)
        {
            try
            {
                //string gameState = string.Empty;
                StringBuilder sb = new StringBuilder();
                //beleirjuk a jatekost
                //gameState += table.Player;
                sb.Append(table.Player);

                //ha van selected oda irjuk
                if (table.AnySelected())
                {
                    sb.Append($" {table.SelectedCell()?.X}{table.SelectedCell()?.Y}");
                }
                sb.AppendLine();

                
                for (int i = 0; i < table.TableSize; i++)
                {
                    for (int j = 0; j < table.TableSize; j++)
                    {
                        sb.Append($"{table.GetGameCellState(i, j)} ");
                    }
                    sb.AppendLine();
                }
                using var f = File.CreateText(path); // uj syntax szerint nem kell zarojel
                await f.WriteAsync(sb.ToString());
                f.Flush();//mindenkeppen kiirjuk amit kell es bezarjuk
                f.Close();
            }
            catch
            {
                throw new BlackholeDataException();
            }
        }

        
    }
}
