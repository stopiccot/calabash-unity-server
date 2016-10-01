When(/^I start the app$/) do
  # Empty for now
end

When(/^device is in landscape mode$/) do
  rotate :right
end

When(/^canvas scaling is enabled$/) do
  backdoor "EnableCanvasScaling", "http://google.com"
end

When(/^canvas is attached to camera$/) do
  backdoor "EnableCanvasCamera", "http://google.com"
end