# AutomationPractice2

AutomationPractice2 is a Selenium-based automation project designed to perform various web automation tasks, including interacting with Google Maps and searching for ZIP codes.

## Project Structure

- **PageObjects/**: Contains page object models for different web pages.
  - `GoogleMapPage.cs`: Page object for Google Maps.
  - `ZipCodePage.cs`: Page object for ZIP code search.
  - `Models/`: Contains data models used in the project.
    - `AdvanceSearchForm.cs`: Model for advanced search form parameters.
    - `ZipCodeSearchResults.cs`: Model for ZIP code search results.
  - **BellatrixEcommerce/**: Contains page objects for Bellatrix eCommerce tests.
    - `MainPage.cs`: Page object for the main shop page.
    - `CartPage.cs`: Page object for the shopping cart.
    - `CheckoutPage.cs`: Page object for the checkout process.
    - `OrderReceivedPage.cs`: Page object for the order confirmation page.
- **TestUtilities/**: Contains utility classes for the project.
  - `Utilities.cs`: Utility methods for taking screenshots and other helper functions.
  - `ProxyService.cs`: Proxy service for capturing and manipulating HTTP traffic.
  - `MeasuredRequest.cs`: Represents a measured HTTP request.
- **TDM/**: Contains test data management classes.
  - `BellatrixTDM.cs`: Provides test data for Bellatrix eCommerce tests.
- **ZipCodeTests.cs**: Contains test cases for ZIP code search functionality.
- **BellatrixEcommerceTests.cs**: Contains test cases for Bellatrix eCommerce functionality.
- **BaseTestClass.cs**: Base class for setting up and tearing down WebDriver instances.

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Chrome browser
- ChromeDriver

### Installing

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/AutomationPractice2.git

2. Navigate to the project directory:
   ```sh
   cd AutomationPractice2

3. Restore the dependencies:
   ```sh
   dotnet restore

4. Running Tests
   ```sh
   dotnet test