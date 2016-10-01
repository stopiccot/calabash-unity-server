Feature: Test feature (landscape)
  Background:
    When I start the app
    And device is in landscape mode

  Scenario: Test scenario (landscape)
    And I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button2"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should see a "NEW TEXT" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should see a "Button3" button

    When I touch "Button3"

    Then I should see "SOME TEXT"

  Scenario: Test scenario with canvas scaling (landscape)
    And canvas scaling is enabled
    And I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button2"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should see a "NEW TEXT" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should see a "Button3" button

    When I touch "Button3"

    Then I should see "SOME TEXT"

  Scenario: Test scenario with canvas attached to camera (landscape)
    And canvas is attached to camera
    And I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button2"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should see a "NEW TEXT" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should see a "Button3" button

    When I touch "Button3"

    Then I should see "SOME TEXT"

  Scenario: Test scenario with canvas both scaled and attached to camera (landscape)
    And canvas scaling is enabled
    And canvas is attached to camera
    And I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should see a "Button2" button
    And I should not see a "Button3" button

    When I touch "Button2"

    Then I should not see "SOME TEXT"
    And I should see a "Button1" button
    And I should not see a "Button2" button
    And I should see a "NEW TEXT" button
    And I should not see a "Button3" button

    When I touch "Button1"

    Then I should see a "Button3" button

    When I touch "Button3"

    Then I should see "SOME TEXT"