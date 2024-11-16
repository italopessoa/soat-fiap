Feature: Create new order

  Scenario: Create a new order successfully
    Given Selected product exists
    And Tracking code is created
    When UseCase is called
    Then it should create the order
    And it should publish integration event
