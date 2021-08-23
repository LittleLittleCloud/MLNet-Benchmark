# generate csv report from train result folder

param(
    [string]$TrainResultFolder,
    [string]$ReportFileName,
    [string]$AutoMLType,
    [string]$TrainingTimeInSeconds,
    [string]$MlnetVersion
)


$mbConfigs = Get-ChildItem $TrainResultFolder -Recurse | where {$_.Extension -eq ".mbconfig"}
$csvs = @()
foreach ($mbConfig in $mbConfigs)
{
    $json = Get-Content $mbConfig.Fullname | ConvertFrom-Json
    $runHistory = $json.RunHistory
    $task = $json.Scenario.ScenarioType
    $metricName = $runHistory.MetricName
    $datasetName = $mbConfig.Basename
    $i = 1
    foreach($trial in $runHistory.Trials)
    {
        Add-Member -InputObject $trial -Name "AutoMLType" -Value $AutoMLType -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "Task" -Value $task -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "MLNetVersion" -Value $MlnetVersion -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "Dataset" -Value $datasetName -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "IterationIndex" -Value $i -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "TrainingTimeInSeconds" -Value $TrainingTimeInSeconds -MemberType NoteProperty
        $i = $i+1
        $csvs += $trial
    }
}


$csvs | Select-Object -Property * -ExcludeProperty Version | Export-Csv -Path $ReportFileName -NoTypeInformation 
