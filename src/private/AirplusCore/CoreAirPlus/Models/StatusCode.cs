using System.ComponentModel.DataAnnotations;
namespace CoreAirPlus.Model
{
    public enum StatusCode
    {
        [Display(Name = "Not Specified")]
        NotSpecified,

        [Display(Name = "Welcome Message")]
        WelcomeMessage,

        [Display(Name = "Reminder To Confirm Message")]
        ReminderToConfirmMessage,

        [Display(Name = "CheckIn Welcome Message")]
        CheckInWelcomeMessage,

        [Display(Name = "CheckOut Welcome Message")]
        CheckOutWelcomeMessage,

        [Display(Name = "Thank You Message")]
        ThankYouMessage,

        [Display(Name = "Write Review")]
        WriteReview
    }
}
