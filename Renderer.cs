using System;
using System.Threading;
using CheatApplication;
using Swed64;

namespace CheatApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the ImGui renderer in a separate thread
            var rendererThread = new Thread(() =>
            {
                Renderer renderer = new Renderer();
                renderer.Start().Wait();
            });
            rendererThread.Start();

            // Initialize SWed64 and get base addresses
            Swed swed = new Swed("CombatMaster");
            IntPtr moduleBase = swed.GetModuleBase("Project.dll");
            IntPtr EngineBase = swed.GetModuleBase("Engine.dll");





            IntPtr ammoInstructionAddress = moduleBase + 0x8B6740;
            IntPtr grenadeInstructionAddress = moduleBase + 0x8B6646;
            IntPtr sprintInstruction = moduleBase + 0x39F4C86;
            IntPtr secAmmoInstruction = moduleBase + 0x8B6847;
            IntPtr RedScore = moduleBase + 0x8E6829;
            IntPtr headbopInstructionAddress = moduleBase + 0x35D522C;
            IntPtr Health = moduleBase + 0x8D6170;
            IntPtr Jump = moduleBase + 0x35D95A0;
            IntPtr separateHealthOffset = moduleBase + 0x8B64CA;  // New health offset

            // Main loop for applying memory patches based on ImGui state
            while (true)
            {
                if (Renderer.headbop)
                {
                    // NOP the headbop instruction (44 89 77 54 -> 90 90 90 90)
                    swed.WriteBytes(headbopInstructionAddress, "90 90 90 90");
                }
                else
                {
                    // Restore the original instruction
                    swed.WriteBytes(headbopInstructionAddress, "44 89 77 54");
                }
                if (Renderer.Jump)
                {
                    swed.WriteBytes(Jump, "90 90 90 90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(Jump, "40 88 BB 8C 00 00 00");
                }

                if (Renderer.health)
                {
                    swed.WriteBytes(Health, "90 90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(Health, "42 89 7C C2 20");
                }
                //
                if (Renderer.freezered)
                {
                    swed.WriteBytes(RedScore, "90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(RedScore, "89 7C C2 20");
                }
                //
                if (Renderer.InfAmmo)
                {
                    swed.WriteBytes(ammoInstructionAddress, "90 90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(ammoInstructionAddress, "42 89 7C C2 20");
                }
                //
                //
                if (Renderer.SecAmmo)
                {
                    swed.WriteBytes(secAmmoInstruction, "90 90 90 90");
                }
                else
                {
                    swed.WriteBytes(secAmmoInstruction, "89 7C C2 20");
                }
                //
                if (Renderer.SetHealthToZero)
                {
                    swed.WriteBytes(separateHealthOffset, new byte[] { 0x00, 0x00, 0x00, 0x00 });  // Set health to 0
                }
                else
                {
                    // Write the original instruction back to avoid crashing
                    swed.WriteBytes(separateHealthOffset, new byte[] { 0x8B, 0x44, 0xC2, 0x20 });  // Default health logic
                }
                if (Renderer.InfGrenades)
                    {
                        swed.WriteBytes(grenadeInstructionAddress, "90 90 90 90");
                    }
                    else
                    {
                        swed.WriteBytes(grenadeInstructionAddress, "89 7C C2 20");
                    }

                    // Optional: Handle FOV adjustment if desired
                    // swed.WriteFloat(fovInstructionAddress, Renderer.FOV);

                    Thread.Sleep(20);
                
            }
        }
    }
}
