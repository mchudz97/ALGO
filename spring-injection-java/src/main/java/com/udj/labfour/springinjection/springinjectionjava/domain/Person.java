package com.udj.labfour.springinjection.springinjectionjava.domain;

import com.udj.labfour.springinjection.springinjectionjava.service.PersonService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.List;

@Component
public class Person implements PersonService {

    public static List<Person> personList = new ArrayList<>();

    @Value("${value.from.file}")
    private String stringValue;

    private String name;
    private String city;
    private int age;


    public Person(){}

    public Person(String name, String city, int age) {
        this.name = name;
        this.city = city;
        this.age = age;
        System.out.println(stringValue);
    }


    @Override
    public void addPerson(Person person) {
        personList.add(person);
    }

    @Override
    public void deletePerson(Person person) {
        Person personToDelete = null;
        for(Person per : personList){
            if(per.equals(person)){
                personToDelete = per;
            }
        }
        if(personToDelete != null){
            personList.remove(personToDelete);
        }
    }

    @Override
    public List<Person> getPersons() {
        return personList;
    }

    @Override
    public Person getPerson(){
        return personList.get(0);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getCity() {
        return city;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public int getAge() {
        return age;
    }

    public void setAge(int age) {
        this.age = age;
    }
}
