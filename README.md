# JiraTools

A command-line tool that imports your time entries from **Clockify** into **Jira** as worklogs.

---

## Configuration

Before using JiraTools, configure your Jira and Clockify connections once. Credentials are stored locally in `%APPDATA%\JiraTools\secrets.json` (Windows) or `~/.config/JiraTools/secrets.json` (Linux).

### Jira

1. **Get your Jira API token**
   Go to [API token](https://id.atlassian.com/manage-profile/security/api-tokens) → **Create API token**.

2. **Configure the connection**
   ```
   jiratools api Jira set --base-url https://your-domain.atlassian.net --user-email you@example.com --api-key <your-api-token>
   ```
   You can update individual values at any time by passing only the option you want to change.

### Clockify

1. **Get your Clockify API key**
   Go to [API keys](https://app.clockify.me/manage-api-keys) → **CREATE NEW**

2. **Configure the connection**
   ```
   jiratools api Clockify set --api-key <your-api-key>
   ```

---

## Mapping Clockify time entry onto Jira worklog

For the import to work, your Clockify workspace must follow these naming conventions:

**Clockify project name** must match your **Jira project key** exactly (e.g. `ADI`).

**Clockify task name** must start with the Jira issue number, optionally followed by a colon and a short description hint:

```
<issueNumber>
<issueNumber>: <hint>
```

Example: `242: Standup`

The following fields are mapped when creating a Jira worklog:

| Jira worklog field | Source in Clockify | Example |
|---|---|---|
| **Issue** | `{project name}-{issue number}` | `ADI-242` |
| **Started** | Time entry start time | Copied as-is |
| **Time spent** | Time entry end − start | Computed duration |
| **Comment** | Time entry description; falls back to task hint if blank; omitted if neither is set | `Standup` |

Overall examples:

- Clockify timesheet row `ADI: 242: Standup` → `ADI-242` Jira worklog with a comment `Standup`.
- Clockify timesheet row `ADI: 242: AVA News` with a comment `Management Bullshit` → `ADI-242` Jira worklog with a comment `Management Bullshit`.

---

## Commands

### `timesheet import`

Reads time entries from Clockify for the specified period and creates the corresponding worklogs in Jira.

```
jiratools timesheet import --source <source> --period <period> [--anchor <date>]
```

| Option | Short | Required | Values | Default | Description |
|---|---|---|---|---|---|
| `--source` | `-s` | ✓ | `Clockify` | — | Source of timesheet data |
| `--period` | `-p` | ✓ | `Day` \| `Week` \| `Month` | — | Time period to import |
| `--anchor` | `-a` | | `YYYY-MM-DD` | today | Reference date for the period |

**Period semantics**

| Period | Imported range |
|---|---|
| `Day` | The anchor date only |
| `Week` | Monday–Sunday of the week containing the anchor date |
| `Month` | The full calendar month of the anchor date |

**Examples**

```
# Import the current week
jiratools timesheet import -s Clockify -p Week

# Import a specific past week
jiratools timesheet import -s Clockify -p Week -a 2025-03-10

# Import a single day
jiratools timesheet import -s Clockify -p Day -a 2025-03-10

# Import a full month
jiratools timesheet import -s Clockify -p Month -a 2025-03-01
```

---

### `api Jira set`

Updates the Jira connection settings. All options are optional — pass only what you want to change.

```
jiratools api Jira set [--base-url <url>] [--user-email <email>] [--api-key <token>]
```

| Option | Short | Description |
|---|---|---|
| `--base-url` | `-u` | Jira instance URL (e.g. `https://your-domain.atlassian.net`) |
| `--user-email` | `-e` | Your Atlassian account email address |
| `--api-key` | `-k` | Your Jira API token |

---

### `api Clockify set`

Updates the Clockify connection settings.

```
jiratools api Clockify set --api-key <token>
```

| Option | Short | Description |
|---|---|---|
| `--api-key` | `-k` | Your Clockify API key |
