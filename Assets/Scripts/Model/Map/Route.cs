/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using System.Collections.Generic;

namespace HeroesArena
{
	// Represents one route on the map as a list of cells in game logic.
	public class Route
	{
        // Stores cells of the route.
		public List<Cell> Cells { get; private set; }

        // Length of the route.
	    public int Length { get { return Cells.Count - 1; } }

        // Adds cell to route.
        public void AddCell(Cell cell)
	    {
            Cells.Add(cell);
	    }

	    // Constructors.
	    public Route()
	    {
            Cells = new List<Cell>();
	    }
        public Route(List<Cell> cells)
        {
            Cells = cells;
        }
    }
}
