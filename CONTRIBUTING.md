# How to contribute

First of all, thank you for wanting to contribute to ConfigR! We really appreciate all the awesome support we get from our community. We want to keep it as easy as possible to contribute changes that get things working in your environment. There are a few guidelines we need contributors to follow to keep the project flowing smoothly.

These guidelines are for code changes but we are always very grateful to receive other forms of contribution, e.g. updates to the [documentation](https://github.com/config-r/config-r/wiki), providing help in the [chatroom](https://jabbr.net/#/rooms/configr), blog posts and samples, Twitter endorsements, etc. :wink:. 

## Preparation

Before starting work on a *functional* change, i.e. a new feature, a change to an existing feature or a bug, please ensure an [issue](https://github.com/config-r/config-r/issues/) has been raised. Indicate your intention to work on the issue by writing a comment against it. This will prevent duplication of effort. If the change is non-trivial, it's usually best to propose a design in the issue comments.

It is **not** necessary to raise an issue for non-functional changes, e.g. refactoring, adding tests, reformatting code, documentation, updating packages, etc.

## Tests

All new features must be covered by feature tests in the `ConfigR.Features.*` projects.

## Spaces not Tabs

Pull requests containing tabs will not be accepted. Make sure you set your editor to replace tabs with spaces. Indents for all file types should be 4 characters wide with the exception of Ruby (e.g. rakefile.rb) which should have indents 2 characters wide.

## Line Endings

The repository is configured to preserve line endings both on checkout and commit (the equivalent of `autocrlf` set to `false`). This means *you* are responsible for line endings. We recommend that you configure your diff viewer so that it does not ignore line endings. Any [wall of pink](http://www.hanselman.com/blog/YoureJustAnotherCarriageReturnLineFeedInTheWall.aspx) pull requests will not be accepted.

## Line Width

Try to keep lines of code no longer than 160 characters wide. This isn't a strict rule. Occasionally a line of code can be more readable if allowed to spill over slightly. A good way to remember this rule is to use the 'Column Guides' feature of the [Productivity Power Tools](http://visualstudiogallery.msdn.microsoft.com/3a96a4dc-ba9c-4589-92c5-640e07332afd) extension for Visual Studio.

## Coding Style

Try to keep your coding style in line with the existing code. It might not exactly match your preferred style but it's better to keep things consistent.

## Code Analysis

Try and avoid introducing code analysis violations. The non-test projects have largely been kept free of code analysis violations and we would like to keep it that way. Any code analysis rule changes or suppressions must be clearly justified.

## Resharper Artifacts

Please do not add Resharper suppressions to code using comments. You may tweak your local Resharper settings but do not commit these to the repo.

## Branches

There are two mainline branches, **master** and **dev**. The **dev** branch is used for development work for the next release. All new features, changes, etc. must be applied to the dev branch. The **master** branch is used for stable releases. Any patches to the current stable release must be applied to master.

## Making Changes

1. [Fork](http://help.github.com/forking/) on GitHub
1. Clone your fork locally
1. Configure the upstream repo (`git remote add upstream git://github.com/config-r/config-r.git`)
1. Checkout the dev branch (`git checkout dev`) or, if you are working on a patch, checkout the master branch (`git checkout master`)  
1. Create a local branch (`git checkout -b my-branch`). The branch name should be descriptive, or it can just be the GitHub issue number which the work relates to, e.g. `123`.
1. Work on your feature
1. Rebase if required (see 'Handling Updates from Upstream' below)
1. Test the build locally by running `rake` (see ['How to build'](https://github.com/config-r/config-r/blob/master/how_to_build.md/))
1. Push the branch up to GitHub (`git push origin my-branch`)
1. Send a Pull Request on GitHub (see 'Sending a Pull Request' below)

You should **never** work on a clone of dev/master, and you should **never** send a pull request from dev/master - always from a branch. The reasons for this are detailed below.

## Handling Updates from Upstream

While you're working away in your branch it's quite possible that your upstream dev/master may be updated. If this happens you should:

(If you are working on patch, replace `dev` with `master` when following these steps.)

1. [Stash](http://progit.org/book/ch6-3.html) any un-committed changes you need to
1. `git checkout dev`
1. `git pull upstream dev`
1. `git checkout my-branch`
1. `git rebase dev my-branch`
1. `git push origin dev` (optional) this keeps the dev branch in your fork up to date

These steps ensure your history is "clean" i.e. you have one branch from dev/master followed by your changes in a straight line. Failing to do this ends up with several "messy" merges in your history, which we don't want. This is the reason why you should always work in a branch and you should never be working in or sending pull requests from dev/master.

If you're working on a long running feature you may want to do this quite often to reduce the risk of tricky merges later on.

## Sending a Pull Request

While working on your feature you may well create several branches, which is fine, but before you send a pull request you should ensure that you have rebased back to a single "Feature branch" - we care about your commits, and we care about your feature branch; but we don't care about how many or which branches you created while you were working on it :-)

When you're ready to go you should confirm that you are up to date and rebased with upstream dev/master (see "Handling Updates from Upstream" above) and then:

1. `git push origin my-branch`
1. Send a descriptive [Pull Request](http://help.github.com/pull-requests/) on GitHub.
  * Make sure the pull request is **from** the branch on your fork **to** the config-r/config-r dev branch (or the config-r/config-r master branch if patching).
  * If your changes relate to a GitHub issue, add the issue number to the pull request description in the format #123.
1. If GitHub determines that the pull request can be merged automatically, a test build will commence approximately one minute after you raise the pull request. The build status will reported on the pull request.
  * If the build fails there may be a problem with your changes which you will have to fix before the pull request can be accepted. Follow the link to the build server (you can either create an account or login as guest) and inspect the build logs to see what caused the failure.
  * Occasionally, build failures may be due to problems on the build server rather than problems in your changes. If you determine this to be the case, please add a comment on the pull request and one of the coordinators will address the problem.

## What Happens Next?

The coordinators will review your pull request and provide any feedback required. If your pull request is accepted, your changes will be included in the next release. Look out for your name in the release notes :trophy:.

If you contributed a new feature or a change to an existing feature then we are always very grateful to receive updates to the [documentation](https://github.com/config-r/config-r/wiki) (*after* the release to prevent confusion).
