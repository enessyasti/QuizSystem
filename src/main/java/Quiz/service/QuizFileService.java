package Quiz.service;

import Quiz.impl.BasicQuiz;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.Reader;
import java.io.Writer;

public class QuizFileService {
    private final Gson gson;

    public QuizFileService() {
        this.gson = new GsonBuilder().setPrettyPrinting().create();
    }

    public BasicQuiz loadQuiz(String filePath) throws IOException {
        try (Reader reader = new FileReader(filePath)) {
            return gson.fromJson(reader, BasicQuiz.class);
        }
    }

    public void saveQuiz(BasicQuiz quiz, String filePath) throws IOException {
        try (Writer writer = new FileWriter(filePath)) {
            gson.toJson(quiz, writer);
        }
    }
}