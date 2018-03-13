//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class Slider : RangeBase {
  internal new static Slider CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new Slider(cPtr, cMemoryOwn);
  }

  internal Slider(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(Slider obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public Slider() {
  }

  protected override IntPtr CreateCPtr(Type type, out bool registerExtend) {
    if ((object)type.TypeHandle == typeof(Slider).TypeHandle) {
      registerExtend = false;
      return NoesisGUI_PINVOKE.new_Slider();
    }
    else {
      return base.CreateExtendCPtr(type, out registerExtend);
    }
  }

  public static DependencyProperty DelayProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_DelayProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IntervalProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IntervalProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IsDirectionReversedProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IsDirectionReversedProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IsMoveToPointEnabledProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IsMoveToPointEnabledProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IsSelectionRangeEnabledProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IsSelectionRangeEnabledProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IsSnapToTickEnabledProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IsSnapToTickEnabledProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty OrientationProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_OrientationProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty SelectionEndProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_SelectionEndProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty SelectionStartProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_SelectionStartProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty TickFrequencyProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_TickFrequencyProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty TickPlacementProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_TickPlacementProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty TicksProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_TicksProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand DecreaseLargeCommand {
    set {
      NoesisGUI_PINVOKE.Slider_DecreaseLargeCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_DecreaseLargeCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand DecreaseSmallCommand {
    set {
      NoesisGUI_PINVOKE.Slider_DecreaseSmallCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_DecreaseSmallCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand IncreaseLargeCommand {
    set {
      NoesisGUI_PINVOKE.Slider_IncreaseLargeCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IncreaseLargeCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand IncreaseSmallCommand {
    set {
      NoesisGUI_PINVOKE.Slider_IncreaseSmallCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_IncreaseSmallCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand MaximizeValueCommand {
    set {
      NoesisGUI_PINVOKE.Slider_MaximizeValueCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_MaximizeValueCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static RoutedCommand MinimizeValueCommand {
    set {
      NoesisGUI_PINVOKE.Slider_MinimizeValueCommand_set(RoutedCommand.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Slider_MinimizeValueCommand_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (RoutedCommand)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public int Delay {
    set {
      NoesisGUI_PINVOKE.Slider_Delay_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      int ret = NoesisGUI_PINVOKE.Slider_Delay_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public int Interval {
    set {
      NoesisGUI_PINVOKE.Slider_Interval_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      int ret = NoesisGUI_PINVOKE.Slider_Interval_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool IsDirectionReversed {
    set {
      NoesisGUI_PINVOKE.Slider_IsDirectionReversed_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      bool ret = NoesisGUI_PINVOKE.Slider_IsDirectionReversed_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool IsMoveToPointEnabled {
    set {
      NoesisGUI_PINVOKE.Slider_IsMoveToPointEnabled_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      bool ret = NoesisGUI_PINVOKE.Slider_IsMoveToPointEnabled_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool IsSelectionRangeEnabled {
    set {
      NoesisGUI_PINVOKE.Slider_IsSelectionRangeEnabled_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      bool ret = NoesisGUI_PINVOKE.Slider_IsSelectionRangeEnabled_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool IsSnapToTickEnabled {
    set {
      NoesisGUI_PINVOKE.Slider_IsSnapToTickEnabled_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      bool ret = NoesisGUI_PINVOKE.Slider_IsSnapToTickEnabled_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public Orientation Orientation {
    set {
      NoesisGUI_PINVOKE.Slider_Orientation_set(swigCPtr, (int)value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      Orientation ret = (Orientation)NoesisGUI_PINVOKE.Slider_Orientation_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public float SelectionStart {
    set {
      NoesisGUI_PINVOKE.Slider_SelectionStart_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      float ret = NoesisGUI_PINVOKE.Slider_SelectionStart_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public float SelectionEnd {
    set {
      NoesisGUI_PINVOKE.Slider_SelectionEnd_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      float ret = NoesisGUI_PINVOKE.Slider_SelectionEnd_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public float TickFrequency {
    set {
      NoesisGUI_PINVOKE.Slider_TickFrequency_set(swigCPtr, value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      float ret = NoesisGUI_PINVOKE.Slider_TickFrequency_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public TickPlacement TickPlacement {
    set {
      NoesisGUI_PINVOKE.Slider_TickPlacement_set(swigCPtr, (int)value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      TickPlacement ret = (TickPlacement)NoesisGUI_PINVOKE.Slider_TickPlacement_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public string Ticks {
    set {
      NoesisGUI_PINVOKE.Slider_Ticks_set(swigCPtr, value != null ? value : string.Empty);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    }
    get {
      IntPtr strPtr = NoesisGUI_PINVOKE.Slider_Ticks_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      string str = Noesis.Extend.StringFromNativeUtf8(strPtr);
      return str;
    }
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.Slider_GetStaticType();
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }


  internal new static IntPtr Extend(string typeName) {
    IntPtr nativeType = NoesisGUI_PINVOKE.Extend_Slider(Marshal.StringToHGlobalAnsi(typeName));
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    return nativeType;
  }
}

}
