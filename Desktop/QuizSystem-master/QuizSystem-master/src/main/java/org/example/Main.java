package org.example;

import Quiz.Interface.Answer;
import Quiz.Interface.Question;
import Quiz.Interface.Quiz;

import Quiz.impl.BasicAnswer;
import Quiz.impl.BasicQuestion;
import Quiz.impl.BasicQuiz;

import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {

        // Create Answers
        Answer a1 = new BasicAnswer("A programming language", true);
        Answer a2 = new BasicAnswer("An operating system", false);

        Answer a3 = new BasicAnswer("1995", true);
        Answer a4 = new BasicAnswer("2005", false);

        // Create Questions
        Question q1 = new BasicQuestion("What is Java?", Arrays.asList(a1, a2));
        Question q2 = new BasicQuestion("When was Java created?", Arrays.asList(a3, a4));

        // Create Quiz
        Quiz quiz = new BasicQuiz("Java Basics Quiz", Arrays.asList(q1, q2));

        runQuiz(quiz);
    }


    private static void runQuiz(Quiz quiz) {

        Scanner scanner = new Scanner(System.in);
        int score = 0;

        System.out.println("=== " + quiz.getTitle() + " ===\n");

        for (Question q : quiz.getQuestions()) {

            System.out.println(q.getPrompt());
            List<Answer> answers = q.getAnswers();

            for (int i = 0; i < answers.size(); i++) {
                System.out.println((i + 1) + ") " + answers.get(i).getContent());
            }

            System.out.print("Your answer: ");
            int choice = scanner.nextInt() - 1;

            if (choice >= 0 && choice < answers.size() && answers.get(choice).isCorrect()) {
                System.out.println("Correct!\n");
                score++;
            } else {
                System.out.println("Wrong!\n");
            }
        }

        System.out.println("Final Score: " + score + "/" + quiz.getQuestions().size());
    }
}
