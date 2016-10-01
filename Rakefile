require "rubygems"
require "colorize"
require "bundler/setup"

def unity_path
  "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
end

namespace :build do
  desc "Build app for device testing"
  task :build_device_app do
    command = "rm -rf CalabashUnityServer/Builds/iOS"
    puts command
    `#{command}`

    command = "#{unity_path} -quit -batchmode -executeMethod AutoBuilder.PerformiOSBuild"
    puts command
    `#{command}`

    if $?.to_i != 0
      puts "Failed to export iOS project".red
      exit(0)
    end

    command = "cd CalabashUnityServer/Builds/iOS && xcodebuild"
    puts command
    `#{command}`
  end

  desc "Build app for simulator testing"
  task :build_app do
    command = "rm -rf CalabashUnityServer/Builds/iOS"
    puts command
    `#{command}`
    
    command = "#{unity_path} -quit -batchmode -executeMethod AutoBuilder.PerformiOSBuildSimulator"
    puts command
    `#{command}`

    if $?.to_i != 0
      puts "Failed to export iOS project".red
      exit(0)
    end

    command = "cd CalabashUnityServer/Builds/iOS && xcodebuild"
    puts command
    `#{command}`
  end
end