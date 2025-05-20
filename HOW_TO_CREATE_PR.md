# How to Create a Pull Request

This document provides instructions on how to push the committed changes to a remote repository and create a pull request.

## Step 1: Push the Branch to the Remote Repository

```bash
# Push the feature branch to the remote repository
git push origin feature/todo-ui-improvements
```

## Step 2: Create the Pull Request

### On GitHub:

1. Navigate to the repository on GitHub
2. You should see a prompt to create a pull request for the recently pushed branch
3. Click on "Compare & pull request"
4. Fill in the PR title: "Implement Todo List with Modern UI"
5. Copy the contents of PR_DESCRIPTION.md into the description area
6. Add reviewers from your team
7. Click "Create pull request"

### On Azure DevOps:

1. Navigate to the repository in Azure DevOps
2. Click on "Create a pull request" 
3. Select your branch as the source branch
4. Select the main branch as the target
5. Fill in the title: "Implement Todo List with Modern UI"
6. Copy the contents of PR_DESCRIPTION.md into the description area
7. Add reviewers from your team
8. Click "Create"

## Step 3: Addressing Feedback

After submitting your PR, you may receive feedback from reviewers. To address feedback:

```bash
# Make needed changes
git add .
git commit -m "Address PR feedback: [brief description]"
git push origin feature/todo-ui-improvements
```

The PR will automatically update with your new changes.

## Step 4: Merge the PR

Once your PR is approved:

1. Make sure CI/CD checks pass (if configured)
2. Click "Merge pull request" (GitHub) or "Complete" (Azure DevOps)
3. Choose the appropriate merge strategy (usually "Squash and merge" or "Create a merge commit")
4. Confirm the merge

## Step 5: Clean Up

After the PR is merged, clean up your local repository:

```bash
git checkout main
git pull
git branch -d feature/todo-ui-improvements
```

This will delete your local feature branch after ensuring your main branch is up to date.

## Best Practices

1. Write clear commit messages
2. Keep PRs focused on a single task or feature
3. Make sure your code passes all tests before submitting
4. Be responsive to reviewer feedback
5. Update the PR description if significant changes are made during review 