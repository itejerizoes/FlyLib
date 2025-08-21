pipeline {
    agent any
    environment {
        SONARQUBE_TOKEN = credentials('c0ad7056b1f1938e19b09cb1f34b4022905e0d24')
    }
    stages {
        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test --no-build --verbosity normal'
            }
        }
        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh 'dotnet sonarscanner begin /k:"FlyLib" /d:sonar.login=$SONARQUBE_TOKEN'
                    sh 'dotnet build'
                    sh 'dotnet sonarscanner end /d:sonar.login=$SONARQUBE_TOKEN'
                }
            }
        }
       }
   }