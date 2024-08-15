# QuestEngine.WebAPI

## Overview

This folder contains the implementation of the Quest Engine Web API, which provides endpoints for handling quest progress and state.


## Endpoints

### POST /api/progress

**Description**: Updates the quest progress for a player.

**Request Body**:
```json
{
  "PlayerId": "string",
  "PlayerLevel": "number",
  "ChipAmountBet": "number"
}
```
**Response**:
```json
{
  "QuestPointsEarned": "number",
  "TotalQuestPercentCompleted": "number",
  "MilestonesCompleted": [
    {
      "MilestoneIndex": "number",
      "ChipsAwarded": "number"
    }
  ]
}
```

### GET /api/state
**Request Parameters**:
- `PlayerId`: The ID of the player.

**Response**:
```json
{
  "TotalQuestPercentCompleted": "number",
  "LastMilestoneIndexCompleted": "number"
}
```
 
 ## Logic

 ### Progress Endpoint
 1. Receive Request: Accepts PlayerId, PlayerLevel, and ChipAmountBet from the request body.
 2. Fetch Config: Retrieves quest configuration values.
 3. Fetch Player State: Retrieves the player's current quest state.
 4. Calculate Quest Points: Computes quest points based on input values and configuration.
 5. Update State: Updates the player's quest state and saves it.
 6. Generate Response: Returns the updated quest state and milestones completed.
 
 ### State Endpoint
 1. Receive Request: Accepts PlayerId as a URL parameter.
 2. Fetch Player State: Retrieves the player's current quest state.
 3. Generate Response: Returns the total quest percent completed and the last milestone index completed.

### Sequence Diagram
[Sequence Diagram](docs/SequenceDiagram.png)

## Quest Configuration

The `QuestConfig` section of the configuration file defines the parameters and milestones for the quest system. Below is an overview of the configuration values:

### Configuration Details

- **RateFromBet**: A multiplier applied to the chip amount bet to calculate quest points.
  - **Type**: `number`
  - **Example**: `0.1`
- **LevelBonusRate**: A multiplier applied to the player's level to calculate additional quest points.
  - **Type**: `number`
  - **Example**: `0.5`
- **TotalQuestPointsToComplete**: The total number of quest points required to complete the quest.
  - **Type**: `number`
  - **Example**: `1000`

### Milestones

Milestones represent a list of checkpoints within the quest that players can achieve. Each milestone has specific points and rewards associated with it.

- **MilestonePointsToComplete**: The number of quest points required to reach this milestone.
  - **Type**: `number`
  - **Example**: `200`
- **ChipsAward**: The number of chips awarded upon reaching this milestone.
  - **Type**: `number`
  - **Example**: `200`

### Example Configuration

```json
{
  "QuestConfig": {
    "RateFromBet": 0.1,
    "LevelBonusRate": 0.5,
    "TotalQuestPointsToComplete": 1000,
    "Milestones": [
      { "MilestonePointsToComplete": 200, "ChipsAward": 200 },
      { "MilestonePointsToComplete": 400, "ChipsAward": 250 },
      { "MilestonePointsToComplete": 700, "ChipsAward": 300 }
    ]
  }
}
