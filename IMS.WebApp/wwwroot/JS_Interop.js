
function preventFormSubmission(formId)
{
    // Get the document element corresponding to the passed in formId.
    // The add a keydown event listener that calls an anonymous function which
    // checks if Enter is pressed, and if so prevents it from causing the form to
    // be submitted like it does by default.
    document.getElementById(`${formId}`).addEventListener('keydown', function (event)
    {
        // Logs the form ID in the browser's developer console.
        console.Log(formId);

        if (event.key == "Enter")
        {
            event.preventDefault();
            return false;
        }
    });
}


// This class is just used to demonstrate one of the new functions added to JSRuntime in
// .NET 10.
class Car
{
    constructor(make, model, year)
    {
        this.male = make;
        this.model = model;
        this.year = year;
    }

    getDescription()
    {
        return `This car is a ${this.year} ${this.make} ${this.model}.`;
    }
}


// Assign the car class to the Window to make it accessible to the C# code.
window.Car = Car;
