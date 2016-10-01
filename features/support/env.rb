require 'calabash-cucumber'
require 'calabash-cucumber/launcher'
require 'calabash-cucumber/cucumber'
require 'calabash-cucumber/calabash_steps'

calabash_launcher = Calabash::Cucumber::Launcher.new

app_path = "./CalabashUnityServer/Builds/iOS/build/Release-iphonesimulator/CalabashUnityServer.app"

options = {
  app: app_path,
  device_target: "iPad Air (10.0)",
  device: "iPad Air (10.0)",
}

ENV["APP_BUNDLE_PATH"] = app_path

calabash_launcher.relaunch(options)

Before do |scenario|
  calabash_launcher.relaunch(options)
end