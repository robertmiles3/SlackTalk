workflow "Client" {
  on = "push"
  resolves = ["Build and Push to NuGet"]
}

# Filter for master branch
action "Filter Master Only" {
  uses = "actions/bin/filter@master"
  args = "branch master"
}

action "Build and Push to NuGet" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  args = "build -t slacktalk:latest --build-arg NUGET_KEY=$NUGET_KEY --build-arg VERSION=\"$(grep '^VERSION_NUMBER' VERSION | cut -d'=' -f2-)\" --build-arg VERSION_SUFFIX=\"$(grep '^VERSION_SUFFIX' VERSION | cut -d'=' -f2-)\" ."
  secrets = ["NUGET_KEY"]
  needs = ["Filter Master Only"]
}
