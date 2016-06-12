namespace Bejewled.Model.Interfaces
{
    using System;

    public interface IView
    {
        int[,] Tiles { get; set; }

        event EventHandler OnLoad;
    }
}