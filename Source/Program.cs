// Decompiled with JetBrains decompiler
// Type: isolateCPU.Program
// Assembly: isolateCPU, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE39AFBC-1B67-4B84-B415-DFA0EEFD39E6
// Assembly location: C:\Users\Dark\Documents\Visual Studio 2015\Projects\isolateCPU\isolateCPU\bin\Release\isolateCPU.exe

using System;
using System.Windows.Forms;

namespace isolateCPU
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new isolateCPU.isolateCPU());
    }
  }
}
