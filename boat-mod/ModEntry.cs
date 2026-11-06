using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace LostIslandBoat
{
    internal sealed class ModEntry : Mod
    {
        private const string IslandName = "Strict.LostIsland_MainIsland";
        private const int BeachX = 88;
        private const int BeachY = 20;
        private const int IslandX = 24;
        private const int IslandY = 28;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Display.RenderedWorld += OnRenderedWorld;
            Monitor.Log("Boat and route menu loaded.", LogLevel.Info);
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || e.Button != SButton.Action)
                return;

            Point player = Game1.player.TilePoint;
            string name = Game1.currentLocation.Name;

            if (name == "Beach" && IsNear(player, BeachX, BeachY))
            {
                Game1.currentLocation.createQuestionDialogue(
                    "Куда отправимся?",
                    new[] {
                        new Response("island", "На Затерянный остров"),
                        new Response("cancel", "Отмена")
                    },
                    OnBeachRouteChosen
                );
                Helper.Input.Suppress(e.Button);
            }
            else if (name == IslandName && IsNear(player, IslandX, IslandY))
            {
                Game1.currentLocation.createQuestionDialogue(
                    "Куда отправимся?",
                    new[] {
                        new Response("beach", "Вернуться на пляж"),
                        new Response("cancel", "Остаться на острове")
                    },
                    OnIslandRouteChosen
                );
                Helper.Input.Suppress(e.Button);
            }
        }

        private static bool IsNear(Point player, int x, int y)
        {
            return Math.Abs(player.X - x) <= 2 && Math.Abs(player.Y - y) <= 2;
        }

        private void OnBeachRouteChosen(Farmer who, string answer)
        {
            if (answer == "island")
                Game1.warpFarmer(IslandName, IslandX, IslandY, false);
        }

        private void OnIslandRouteChosen(Farmer who, string answer)
        {
            if (answer == "beach")
                Game1.warpFarmer("Beach", BeachX, BEACH_RETURN_Y, false);
        }

        private const int BEACH_RETURN_Y = 21;

        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            string name = Game1.currentLocation.Name;
            if (name != "Beach" && name != IslandName)
                return;

            Vector2 tile = name == "Beach"
                ? new Vector2(BeachX, BeachY - 2)
                : new Vector2(IslandX, IslandY - 2);
            Vector2 p = Game1.GlobalToLocal(Game1.viewport, tile * 64f);
            var b = e.SpriteBatch;
            Texture2D pixel = Game1.staminaRect;

            // Stylized top-down passenger boat, drawn from simple pixel blocks.
            b.Draw(pixel, new Rectangle((int)p.X + 4, (int)p.Y + 35, 112, 27), new Color(65, 42, 31));
            b.Draw(pixel, new Rectangle((int)p.X + 10, (int)p.Y + 30, 100, 22), new Color(145, 78, 39));
            b.Draw(pixel, new Rectangle((int)p.X + 14, (int)p.Y + 28, 92, 5), new Color(226, 164, 83));
            b.Draw(pixel, new Rectangle((int)p.X + 20, (int)p.Y + 38, 80, 3), new Color(196, 127, 66));
            b.Draw(pixel, new Rectangle((int)p.X + 20, (int)p.Y + 46, 80, 3), new Color(196, 127, 66));
            b.Draw(pixel, new Rectangle((int)p.X + 29, (int)p.Y + 10, 5, 39), new Color(91, 53, 30));
            b.Draw(pixel, new Rectangle((int)p.X + 88, (int)p.Y + 10, 5, 39), new Color(91, 53, 30));
            b.Draw(pixel, new Rectangle((int)p.X + 31, (int)p.Y + 7, 60, 20), new Color(229, 218, 184));
            b.Draw(pixel, new Rectangle((int)p.X + 43, (int)p.Y + 7, 8, 20), new Color(55, 111, 157));
            b.Draw(pixel, new Rectangle((int)p.X + 63, (int)p.Y + 7, 8, 20), new Color(55, 111, 157));
            b.Draw(pixel, new Rectangle((int)p.X + 83, (int)p.Y + 7, 7, 20), new Color(55, 111, 157));
            b.Draw(pixel, new Rectangle((int)p.X + 54, (int)p.Y + 30, 24, 16), new Color(105, 68, 39));
            b.Draw(pixel, new Rectangle((int)p.X + 59, (int)p.Y + 33, 14, 7), new Color(119, 176, 198));
            b.Draw(pixel, new Rectangle((int)p.X + 101, (int)p.Y + 2, 3, 27), new Color(83, 49, 29));
            b.Draw(pixel, new Rectangle((int)p.X + 104, (int)p.Y + 3, 17, 9), new Color(43, 97, 151));
        }
    }
}