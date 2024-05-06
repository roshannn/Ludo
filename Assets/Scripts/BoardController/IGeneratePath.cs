using NUnit.Framework;
using System.Collections.Generic;

//Use this interface to access generating the path on the board
public interface IGeneratePath {
    public List<Tile> GeneratePath(PlayerType playerType);
}
