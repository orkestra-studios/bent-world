#if UNITY_IOS 
using System.Runtime.InteropServices;
#endif

public static class HapticEngine
{
    [DllImport ( "__Internal" )] private static extern void _IOSNotificationFeedbackError ();
    [DllImport ( "__Internal" )] private static extern void _IOSNotificationFeedbackSuccess ();
    [DllImport ( "__Internal" )] private static extern void _IOSNotificationFeedbackWarning ();
    [DllImport ( "__Internal" )] private static extern void _IOSImpactFeedbackLight ();
    [DllImport ( "__Internal" )] private static extern void _IOSImpactFeedbackMedium ();
    [DllImport ( "__Internal" )] private static extern void _IOSImpactFeedbackHeavy ();
    [DllImport ( "__Internal" )] private static extern void _IOSImpactFeedbackSoft (); // IOS +13.0, fallback _IOSImpactFeedbackLight
    [DllImport ( "__Internal" )] private static extern void _IOSImpactFeedbackRigid (); // IOS +13.0, fallback _IOSImpactFeedbackHeavy
    [DllImport ( "__Internal" )] private static extern void _IOSSelectionFeedback (); 
    public static void NotificationFeedbackError () {
    #if UNITY_IOS && !UNITY_EDITOR
        _IOSNotificationFeedbackError ();
    #endif
    }
    public static void NotificationFeedbackSuccess () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSNotificationFeedbackSuccess ();
    #endif
    }

    public static void NotificationFeedbackWarning () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSNotificationFeedbackWarning ();
    #endif
    }

    public static void ImpactFeedbackLight () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSImpactFeedbackLight ();
    #endif
    }

    public static void ImpactFeedbackMedium () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSImpactFeedbackMedium ();
    #endif
    }

    public static void ImpactFeedbackHeavy () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSImpactFeedbackHeavy ();
    #endif
    }

    public static void ImpactFeedbackSoft () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSImpactFeedbackSoft ();
    #endif
    }

    public static void ImpactFeedbackRigid () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSImpactFeedbackRigid ();
    #endif
    }

    public static void SelectionFeedback () {
    #if UNITY_IOS && !UNITY_EDITOR
	    _IOSSelectionFeedback ();
    #endif
    }
}
