package org.example;

import Quiz.Interface.*;
import Quiz.impl.*;
import java.util.*;

public class Main {

    public static void main(String[] args) {

        // Cevapları oluşturuyoruz
        Answer a1 = new BasicAnswer("A programming language", true);
        Answer a2 = new BasicAnswer("An operating system", false);
        Answer a3 = new BasicAnswer("1995", true);
        Answer a4 = new BasicAnswer("2005", false);
        Answer a5 = new BasicAnswer("1989", false);

        // Soruları cevap listeleriyle birlikte oluşturuyoruz
        Question q1 = new BasicQuestion("What is Java?", Arrays.asList(a1, a2));
        Question q2 = new BasicQuestion("When was Java first released?", Arrays.asList(a3, a4, a5));

        // Tüm quiz'i oluşturuyoruz
        Quiz quiz = new BasicQuiz("Java Basics Quiz", Arrays.asList(q1, q2));

        // Quiz'i çalıştır
        runQuiz(quiz);
    }

    // Quiz'i kullanıcıya sorar ve puan hesaplar
    private static void runQuiz(Quiz quiz) {
        Scanner scanner = new Scanner(System.in);
        int score = 0;

        // Quiz başlığını yazdır
        System.out.println("=== " + quiz.getTitle() + " ===\n");

        // Her soruyu sırayla işle
        for (Question question : quiz.getQuestions()) {
            System.out.println(question.getPrompt());                    // Soruyu göster

            List<? extends Answer> answers = question.getAnswers();

            // Tüm seçenekleri numaralandırarak yazdır
            for (int i = 0; i < answers.size(); i++) {
                System.out.println((i + 1) + ") " + answers.get(i).getContent());
            }

            // Kullanıcıdan cevap al (1, 2, 3... şeklinde)
            System.out.print("Your answer: ");
            int choice = scanner.nextInt() - 1;

            // Doğru indeksi ile karşılaştır
            if (choice == question.getCorrectAnswerIndex()) {
                System.out.println("Correct!\n");
                score++;
            } else {
                System.out.println("Wrong! Correct answer is: " +
                        answers.get(question.getCorrectAnswerIndex()).getContent() + "\n");
            }
        }

        // Son puanı göster
        System.out.println("Quiz finished! Your score: " + score + "/" + quiz.getQuestions().size());
        scanner.close();
    }
}