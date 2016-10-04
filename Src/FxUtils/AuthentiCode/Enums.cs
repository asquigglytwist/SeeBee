
namespace SeeBee.FxUtils.AuthentiCode
{
    #region Enums
    #region AllocMethod
    enum AllocMethod
    {
        HGlobal,
        CoTaskMem
    };
    #endregion

    #region UnionChoice
    enum UnionChoice
    {
        File = 1,
        Catalog,
        Blob,
        Signer,
        Cert
    };
    #endregion

    #region UiChoice
    enum UiChoice
    {
        All = 1,
        NoUI,
        NoBad,
        NoGood
    };
    #endregion

    #region RevocationCheckFlags
    enum RevocationCheckFlags
    {
        None = 0,
        WholeChain
    };
    #endregion

    #region StateAction
    enum StateAction
    {
        Ignore = 0,
        Verify,
        Close,
        AutoCache,
        AutoCacheFlush
    };
    #endregion

    #region TrustProviderFlags
    enum TrustProviderFlags
    {
        UseIE4Trust = 1,
        NoIE4Chain = 2,
        NoPolicyUsage = 4,
        RevocationCheckNone = 16,
        RevocationCheckEndCert = 32,
        RevocationCheckChain = 64,
        RecovationCheckChainExcludeRoot = 128,
        Safer = 256,
        HashOnly = 512,
        UseDefaultOSVerCheck = 1024,
        LifetimeSigning = 2048
    };
    #endregion

    #region UIContext
    enum UIContext
    {
        Execute = 0,
        Install
    };
    #endregion
    #endregion
}
