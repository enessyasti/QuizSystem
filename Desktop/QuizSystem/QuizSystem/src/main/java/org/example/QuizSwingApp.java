package org.example;

import Quiz.Interface.Answer;
import Quiz.Interface.Question;
import Quiz.impl.BasicQuiz;
import javax.swing.*;
import java.awt.*;
import java.util.List;

public class QuizSwingApp extends JFrame {
    private final BasicQuiz quiz;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private final JLabel questionLabel;
    private final JPanel optionsPanel;

    public QuizSwingApp(BasicQuiz quiz) {
        this.quiz = quiz;

        setTitle(quiz.getTitle());
        setSize(600, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout());

        questionLabel = new JLabel("", SwingConstants.CENTER);
        questionLabel.setFont(new Font("Arial", Font.BOLD, 16));
        questionLabel.setBorder(BorderFactory.createEmptyBorder(20, 20, 20, 20));
        add(questionLabel, BorderLayout.NORTH);

        optionsPanel = new JPanel();
        optionsPanel.setLayout(new GridLayout(0, 1, 10, 10));
        optionsPanel.setBorder(BorderFactory.createEmptyBorder(20, 50, 20, 50));
        add(optionsPanel, BorderLayout.CENTER);

        loadCurrentQuestion();
    }

    private void loadCurrentQuestion() {
        optionsPanel.removeAll();

        if (currentQuestionIndex >= quiz.getQuestions().size()) {
            showResult();
            return;
        }

        Question question = quiz.getQuestions().get(currentQuestionIndex);
        questionLabel.setText("<html><center>" + question.getPrompt() + "</center></html>");

        List<? extends Answer> answers = question.getAnswers();
        for (int i = 0; i < answers.size(); i++) {
            Answer answer = answers.get(i);
            JButton optionButton = new JButton(answer.getContent());
            optionButton.setFont(new Font("Arial", Font.PLAIN, 14));

            int finalIndex = i;
            optionButton.addActionListener(e -> handleAnswer(finalIndex));

            optionsPanel.add(optionButton);
        }

        optionsPanel.revalidate();
        optionsPanel.repaint();
    }

    private void handleAnswer(int selectedIndex) {
        Question question = quiz.getQuestions().get(currentQuestionIndex);

        if (selectedIndex == question.getCorrectAnswerIndex()) {
            score++;
            JOptionPane.showMessageDialog(this, "Correct Answer!");
        } else {
            Answer correct = question.getAnswers().get(question.getCorrectAnswerIndex());
            JOptionPane.showMessageDialog(this, "Wrong! Correct: " + correct.getContent());
        }

        currentQuestionIndex++;
        loadCurrentQuestion();
    }

    private void showResult() {
        questionLabel.setText("Quiz Finished!");
        optionsPanel.removeAll();

        JLabel scoreLabel = new JLabel("Your Score: " + score + " / " + quiz.getQuestions().size());
        scoreLabel.setFont(new Font("Arial", Font.BOLD, 20));
        scoreLabel.setHorizontalAlignment(SwingConstants.CENTER);

        optionsPanel.setLayout(new BorderLayout());
        optionsPanel.add(scoreLabel, BorderLayout.CENTER);

        optionsPanel.revalidate();
        optionsPanel.repaint();
    }
}