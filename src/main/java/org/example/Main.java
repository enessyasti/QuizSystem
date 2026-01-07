package org.example;

import Quiz.impl.BasicQuiz;
import Quiz.service.QuizFileService;
import javax.swing.*;
import java.io.IOException;

public class Main {
    public static void main(String[] args) {
        QuizFileService fileService = new QuizFileService();
        BasicQuiz quiz = null;
        String jsonPath = "quiz.json";

        try {
            quiz = fileService.loadQuiz(jsonPath);
        } catch (IOException e) {
            JOptionPane.showMessageDialog(null, "Error loading file: " + e.getMessage());
            return;
        }

        if (quiz != null) {
            BasicQuiz finalQuiz = quiz;
            SwingUtilities.invokeLater(() -> {
                QuizSwingApp app = new QuizSwingApp(finalQuiz);
                app.setVisible(true);
            });
        }
    }
}