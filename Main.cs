using System;
using System.Runtime.InteropServices;
using System.Threading;
using ImGuiNET;
using ClickableTransparentOverlay;
using System.Diagnostics;
using System.Text;

namespace CheatApplication
{
    public class Renderer : Overlay
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        public static bool InfAmmo = false;
        public static bool SecAmmo = false;
        public static bool InfGrenades = false;
        public static bool freezered = false;
        public static bool respawnInstruction = false;
        public static float FOV = 65f;
        public static bool SuperJump = false;
        public static bool health = false;
        public static bool headbop = false;
        public static bool Jump = false;
        public static bool Win = false;
        public static bool SetHealthToZero = false;

        public static int JumpHeight = 100;

        private bool showKeyWindow = true;
        private bool showMainWindow = false;
        private bool styleInitialized = false;
        private byte[] inputBuffer = new byte[100];
        private const string correctKey = "Vaquent";
        private const string AdminKey = "Ad"; // Change this to your desired key

        protected override void Render()
        {
            if (showMainWindow) 
            {
                if ((GetAsyncKeyState(0x2D) & 0x8000) != 0)
                {
                    showMainWindow = !showMainWindow;
                    Thread.Sleep(150);
                }

            }



            if (showKeyWindow)
            {
                if (!styleInitialized)
                {
                    InitializeStyle();
                    styleInitialized = true;
                }

                ImGui.Begin("Key Entry", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);

                ImGui.TextColored(new System.Numerics.Vector4(0.0f, 1.0f, 0.0f, 1.0f), "click the button to open the cheat");






                if (ImGui.Button("Enter"))
                {

                    {
                        showKeyWindow = false;
                        showMainWindow = true;
                    }
                }
                ImGui.SameLine();

                ImGui.End();
            }

            if (showMainWindow)
            {
                if (!styleInitialized)
                {
                    InitializeStyle();
                    styleInitialized = true;
                }

                // Centered Title
                ImGui.SetCursorPosX((ImGui.GetWindowSize().X - ImGui.CalcTextSize("CMCHEAT Menu").X) / 2);
                ImGui.TextColored(new System.Numerics.Vector4(0.9f, 0.7f, 0.3f, 1.0f), "CMCHEAT Menu");

                ImGui.Separator();
                ImGui.BeginTabBar("TabBar");

                if (ImGui.BeginTabItem("Universal"))
                {
                    ImGui.TextColored(new System.Numerics.Vector4(0.6f, 0.8f, 0.3f, 1.0f), "Works in both modes");
                    ImGui.Separator();

                    ImGui.Checkbox("Infinite ammo | PRIMARY GUNS ONLY", ref InfAmmo);
                    ImGui.Checkbox("Infinite ammo | SECONDARY GUNS ONLY", ref SecAmmo);
                    ImGui.Checkbox("Infinite Grenades", ref InfGrenades);
                    ImGui.Spacing();


                    ImGui.Checkbox("Infinite jump", ref Jump);
                    if (ImGui.Button("Remove head bop"))
                    {
                        headbop = !headbop;  // Toggle headbop on button press
                    }


                    ImGui.Spacing();

                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Custom Games"))
                {
                    ImGui.TextColored(new System.Numerics.Vector4(0.9f, 0.7f, 0.3f, 1.0f), "Custom Games Settings");
                    ImGui.Separator();
                    ImGui.Checkbox("Infinite Health", ref health);
                    ImGui.Checkbox("Freeze other team's score", ref freezered);
                    ImGui.Spacing();

                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();

                ImGui.Separator();

                if (ImGui.Button("Unload"))
                {
                    Environment.Exit(0);
                }

                ImGui.SameLine();

                if (ImGui.Button("Hide"))
                {
                    showMainWindow = false;
                }

                ImGui.SameLine();

                ImGui.End();
            }
        }

        private void InitializeStyle()
        {
            var style = ImGui.GetStyle();
            style.WindowRounding = 5.0f;
            style.FrameRounding = 5.0f;
            style.GrabRounding = 5.0f;
            style.WindowPadding = new System.Numerics.Vector2(10, 10);
            style.FramePadding = new System.Numerics.Vector2(10, 5);
            style.ItemSpacing = new System.Numerics.Vector2(10, 8);

            // Colors
            var colors = style.Colors;
            colors[(int)ImGuiCol.WindowBg] = new System.Numerics.Vector4(0.1f, 0.1f, 0.1f, 1.0f);
            colors[(int)ImGuiCol.TitleBg] = new System.Numerics.Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.TitleBgActive] = new System.Numerics.Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.FrameBg] = new System.Numerics.Vector4(0.3f, 0.3f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.FrameBgHovered] = new System.Numerics.Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            colors[(int)ImGuiCol.FrameBgActive] = new System.Numerics.Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            colors[(int)ImGuiCol.CheckMark] = new System.Numerics.Vector4(0.9f, 0.7f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.Button] = new System.Numerics.Vector4(0.35f, 0.35f, 0.35f, 1.0f);
            colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0.45f, 0.45f, 0.45f, 1.0f);
            colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0.55f, 0.55f, 0.55f, 1.0f);
            colors[(int)ImGuiCol.Separator] = new System.Numerics.Vector4(0.6f, 0.6f, 0.6f, 1.0f);
        }

        private void OpenWebsite(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open the website: " + ex.Message);
            }
        }
    }
}
