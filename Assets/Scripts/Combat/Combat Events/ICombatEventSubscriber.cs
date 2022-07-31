interface ICombatEventSubscriber{
    /// <summary>
    /// The subscriber will subscribe to any relevant combat events.
    /// </summary>
    void Subscribe();

    /// <summary>
    /// Remove all subscriptions.
    /// </summary>
    void Unsubscribe();
}