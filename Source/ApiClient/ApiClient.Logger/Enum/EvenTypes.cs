namespace ApiClient.Logger.Enum
{
    /// <summary>
    /// Types of the messages to identify if the log is related to an exception or actions performed
    /// Error: Generally is used when is an exception in the logic, the catch should invoke the Logger service for futher investigation
    /// Warning: In the case that there is not an error but is performing task that should not been done
    /// Information: It should be used in cases when the process flow needs to be traced
    /// </summary>
    public enum EvenTypes
    {
        Information=1,
        Warning=2,
        Error=3
    }
}
