# Define a variável para o processo a ser iniciado
$processApi = Start-Process -FilePath "kubectl.exe" -ArgumentList "port-forward svc/svc-api 30000:80" -NoNewWindow
$processSeq = Start-Process -FilePath "kubectl.exe" -ArgumentList "port-forward svc/svc-seq 30008:80" -NoNewWindow
# Verifica se o processo foi iniciado com sucesso
if ($processApi.ExitCode -eq 0) {
  
  # Adiciona um manipulador de eventos para o evento de encerramento do prompt
  Register-ObjectEvent -InputObject $Host -EventName SessionEnding -Action {
  
    # Encerra o processo minikube
    Stop-Process -Id $processApi.Id -Force
    Stop-Process -Id $processSeq.Id -Force
    # Remove o manipulador de eventos
    Remove-Event -SourceIdentifier $event.SourceIdentifier
  }

  Write-Host "htto://localhost:30000/swagger"
  Write-Host "htto://localhost:30008"
  Write-Host "Servicos iniciados em segundo plano. Pressione Ctrl+C para encerrar."

  # Exibe uma mensagem informando que o minikube está sendo executado em segundo plano
  Write-Host "Servicos iniciados em segundo plano. Pressione Ctrl+C para encerrar."

  # Mantém o prompt aberto até que o usuário pressione Ctrl+C
  while ($true) {
    # Aguarda um evento de entrada do usuário
    $null = Read-Host -Prompt ""
  }
} else {
  # Exibe uma mensagem de erro se o processo não foi iniciado com sucesso
  Write-Host "Erro ao iniciar kubectl."
}